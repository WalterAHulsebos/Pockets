using UnityEngine;

#if ODIN_INSPECTOR
using ScriptableObject = Sirenix.OdinInspector.SerializedScriptableObject; 
#endif

/// <summary>
/// A ScriptableObject which caches and loads itself. 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MemorizedScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private const string RESOURCES_FOLDER_NAME = "MemorizedScriptableObjects";

    [SerializeField] private string _name;
    
    private T a_CachedObject;
    public T Load
    {
        get
        {    
            if (a_CachedObject != null) return a_CachedObject;

            T obj = Resources.Load(path: $"{RESOURCES_FOLDER_NAME}/{_name}") as T;
            
            //return _CachedObject = obj ?? CreateInstance<T>();
            return a_CachedObject = obj ? obj : CreateInstance<T>();
        }
    }
}
