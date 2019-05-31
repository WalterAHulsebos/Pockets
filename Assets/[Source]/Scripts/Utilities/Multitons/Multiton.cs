using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

namespace Utilities
{
    public abstract class Multiton<T> : MonoBehaviour where T : Multiton<T>
    {
        protected static int Length;

        /// <summary>
        /// The static reference to the Instances
        /// </summary>
        private static Dictionary<Type, object> InstancesDictionary { get; set; } = new Dictionary<Type, object>();

        public static List<T> Instances
        {
            get
            {
                object instances;
                InstancesDictionary.TryGetValue(typeof(T), out instances);
                return (List<T>)instances;
            }
            set
            {
                InstancesDictionary.TryGetValue(typeof(T), out object instances);
                instances = value;
            }
        }

        /// <summary>
        /// Gets whether Instances of this multiton exist
        /// </summary>
        public static bool InstanceExists
        {
            get {
                bool typeExists = InstancesDictionary.TryGetValue(typeof(T), out object instances);
                if (!typeExists)
                    return false;
                return ((List<T>)instances).Count > 0;
            }
        }

        /// <summary>
        /// OnEnable method to associate Multiton with Instances
        /// </summary>
        protected virtual void OnEnable()
        {
            object instances;
            bool typeExists = InstancesDictionary.TryGetValue(typeof(T), out instances);
            if (!typeExists)
            {
                instances = new List<T>(Length);
                InstancesDictionary.Add(typeof(T), instances);
            }

            ((List<T>)instances).Add((T)this);
        }

        /// <summary>
        /// OnDisable method to clear Singleton association
        /// </summary>
        protected virtual void OnDisable()
        {
            InstancesDictionary.TryGetValue(typeof(T), out object instances);
            ((List<T>)instances).Remove((T)this);
        }
    }
}