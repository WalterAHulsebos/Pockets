namespace Utilities.CGTK.CGPooling
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Utilities.Extensions;

    public static class PoolExtension
    {
        /// <summary>
        /// Instead of constantly creating threads, this threadpooler will create nonactive threads which can be activated and deactivated
        /// for a fraction of the cost. This pooler is mainly used for thread that are processing and returning information every frame.
        /// </summary>
        public class ThreadPooler
        {
            private List<ThreadPoolable> active, nonActive, waitList;

            /// <param name="size">The maximum amount of threads that can run at the same time</param>
            public ThreadPooler(int size)
            {
                active = new List<ThreadPoolable>(size);
                nonActive = new List<ThreadPoolable>(size);
                waitList = new List<ThreadPoolable>(size);

                for (int i = 0; i < size; i++)
                    nonActive.Add(new ThreadPoolable());
            }

            /// <summary>
            /// Add a new thread that will run continuously.
            /// </summary>
            /// <param name="collector">This will gather data from the main thread (Executed on main thread).</param>
            /// <param name="handler">This will process the data given by the collector (Executed on subthread).</param>
            /// <param name="processor">This will put the processed data back on the main thread (Executed on main thread).</param>
            /// <param name="id">Used to identify thread, used for ending single target threads.</param>
            public void Add(Func<object> collector, Func<object, object> handler, Action<object> processor, string id)
            {
                ThreadPoolable threadPoolable = nonActive.Pop();
                threadPoolable.Fill(collector, handler, processor, id);
                waitList.Add(threadPoolable);
            }

            /// <summary>
            /// Remove target thread with target id.
            /// </summary>
            public void Remove(string id)
            {
                Func<ThreadPoolable, bool> action = x => x.ID == id;
                ThreadPoolable threadPoolable = active.Get(action);

                threadPoolable.Clear();
                active.Remove(threadPoolable);
                nonActive.Add(threadPoolable);
            }

            /// <summary>
            /// End all current running threads and delete them.
            /// </summary>
            public void Clear()
            {
                foreach (ThreadPoolable poolable in active)
                    poolable.Clear();
                active.Clear();
                nonActive.Clear();
            }

            /// <summary>
            /// Collect data from the main thread which will be processed in the subthreads.
            /// </summary>
            public void Collect()
            {
                foreach (ThreadPoolable poolable in active)
                    poolable.Collect();

                foreach (ThreadPoolable poolable in waitList)
                {
                    active.Add(poolable);
                    poolable.Collect();
                    poolable.Continue();
                }

                if (waitList.Count > 0)
                    waitList.Clear();
            }

            /// <summary>
            /// Forward all processed data from the subthreads in the main thread.
            /// </summary>
            public void Process()
            {
                foreach (ThreadPoolable poolable in active)
                {
                    poolable.Process();
                    poolable.Continue();
                }
            }

            private class ThreadPoolable
            {
                public string ID { get; private set; }

                private object input, output;
                private Thread thread;
                private EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

                private Func<object> collector;
                private Func<object, object> handler;
                private Action<object> processor;

                public void Collect() => input = collector();
                public void Continue() => handle.Set();

                public ThreadPoolable()
                {
                    void Loopable()
                    {
                        while (true)
                        {
                            handle.WaitOne();
                            lock (input)
                                output = handler(input);
                        }
                    }

                    thread = new Thread(Loopable) { IsBackground = true };
                    thread.Start();
                }

                public void Process()
                {
                    lock (output)
                        processor(output);
                }

                public void Fill(Func<object> collector, Func<object, object> handler, Action<object> processor, string id)
                {
                    this.collector = collector;
                    this.handler = handler;
                    this.processor = processor;
                    ID = id;
                }

                public void Clear()
                {
                    thread.Abort();
                    handle.Set();
                    input = null;
                    output = null;
                }
            }
        }
    }

    /// <summary>
    /// This will cache classes so that you wont have to instantiate them during runtime, which saves a lot of performance,
    /// not to mention that the garbage collector will think you're a good boy.
    /// </summary>
    public static class ClassPooler
    {
        private static Dictionary<Type, List<object>> pool = new Dictionary<Type, List<object>>();

        // Normies

        /// <summary>
        /// Pool everything in target list.
        /// </summary>
        public static void Pool<T>(this List<T> items) where T : class
        {
            if (!pool.TryGetValue(typeof(T), out List<object> poolList))
            {
                poolList = new List<object>(items.Count);
                pool.Add(typeof(T), poolList);
            }
            foreach (T item in items)
                poolList.Add(item);
        }

        /// <summary>
        /// Pool target item.
        /// </summary>
        public static void Pool<T>(this T item) where T : class
        {
            if (!pool.TryGetValue(typeof(T), out List<object> poolList))
            {
                poolList = new List<object>();
                pool.Add(typeof(T), poolList);
            }
            poolList.Add(item);
        }

        /// <summary>
        /// Get item of target T from pool.
        /// </summary>
        public static T Get<T>()
        {
            pool.TryGetValue(typeof(T), out List<object> poolList);
            return (T)poolList.Pop();
        }

        /// <summary>
        /// Put x items in target list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="amount"></param>
        public static void Get<T>(this List<T> list, int amount)
        {
            pool.TryGetValue(typeof(T), out List<object> poolList);
            for (int i = 0; i < amount; i++)
                list.Add((T)poolList.Pop());
        }

        // Advanced

        /// <summary>
        /// Set target list of objects to be a pool list.
        /// </summary>
        public static void SetPoolList<T>(this List<object> items) => pool.Add(typeof(T), items);

        public static List<object> SetPoolList<T>(int amount) where T : new()
        {
            List<object> poolList = new List<object>(amount);
            for (int i = 0; i < amount; i++)
                poolList.Add(new T());
            return poolList;
        }

        /// <summary>
        /// Get direct pool list consisting of objects.
        /// </summary>
        public static List<object> GetPoolList<T>()
        {
            pool.TryGetValue(typeof(T), out List<object> poolList);
            return poolList;
        }
    }
}