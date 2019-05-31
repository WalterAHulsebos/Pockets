using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Utilities.Extensions;

namespace Utilities.CGTK.CGPooling
{
    public static class GameObjectPooler
    {
        public class Pool
        {
            public int Count => nonActive.Count;

            private Transform parent;

            public List<GameObject> active, nonActive;
            public Pool(List<GameObject> nonActive, List<GameObject> active = null)
            {
                this.active = active ?? new List<GameObject>(nonActive.Count);
                this.nonActive = nonActive;

                parent = new GameObject().transform;
                parent.name = "Pool Database";
            }

            public GameObject Get(bool setActive = true)
            {
                GameObject obj = nonActive.Pop();
                obj.SetActive(setActive);

                active.Add(obj);
                nonActive.Remove(obj);

                if(obj.transform.parent == parent)
                    obj.transform.parent = null;
                return obj;
            }

            public void Add(GameObject obj, bool unparent = false)
            {
                obj.SetActive(false);

                active.Remove(obj);           
                nonActive.Add(obj);

                if(unparent)
                    obj.transform.SetParent(parent);
            }

            public void PoolAll(bool unparent = false)
            {
                int length = active.Count - 1;
                while (length-- >= 0)
                {
                    GameObject obj = active.Pop();
                    obj.SetActive(false);
                    if (unparent)
                        obj.transform.SetParent(parent);
                    nonActive.Add(obj);                    
                }
            }

            public void DestroyAll()
            {
                int length = active.Count;
                while (length-- >= 0)
                    MonoBehaviour.Destroy(active[length]);
                length = nonActive.Count;
                while (length-- >= 0)
                    MonoBehaviour.Destroy(nonActive[length]);
            }
        }

        private static Dictionary<Type, Pool> poolType = new Dictionary<Type, Pool>();
        private static Dictionary<string, Pool> poolTag = new Dictionary<string, Pool>();

        #region Pool Type
        public static Pool AddToPool<T>(this List<GameObject> poolList, bool unparent = false)
        {
            Pool pool = new Pool(new List<GameObject>(poolList.Count), poolList);
            poolType.Add(typeof(T), pool);
            pool.PoolAll(unparent);
            return pool;
        }

        public static void AddToPool<T>(this GameObject single, bool unparent = false)
        {
            poolType.TryGetValue(typeof(T), out Pool pool);
            pool.Add(single, unparent);
        }

        public static Pool GetPool<T>()
        {
            poolType.TryGetValue(typeof(T), out Pool pool);
            return pool;
        }

        public static GameObject GetSingle<T>()
        {
            poolType.TryGetValue(typeof(T), out Pool pool);
            return pool.Get();
        }
        #endregion

        #region Pool Tag
        public static Pool AddToPool(this List<GameObject> poolList, string tag, bool unparent = false)
        {
            Pool pool = new Pool(new List<GameObject>(poolList.Count), poolList);
            poolTag.Add(tag, pool);
            pool.PoolAll(unparent);
            return pool;
        }

        public static void AddToPool(this GameObject single, string tag, bool unparent = false)
        {
            poolTag.TryGetValue(tag, out Pool pool);
            pool.Add(single, unparent);
        }

        public static Pool GetPool(string tag)
        {
            poolTag.TryGetValue(tag, out Pool pool);
            return pool;
        }

        public static GameObject GetSingle(string tag)
        {
            poolTag.TryGetValue(tag, out Pool pool);
            return pool.Get();
        }
        #endregion
    }
}