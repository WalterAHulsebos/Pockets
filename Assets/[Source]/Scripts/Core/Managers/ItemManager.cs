using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    /* TODO: [Wybren]
     * Keep track of all global items
     * Add items to world\
     * remove / request items from world
     */

    public List<Item> globalItems = new List<Item>();
    [SerializeField] private Object[] allTypes;

    [HideInInspector] public delegate void DegradeCallback();
    public event DegradeCallback degradeCallback;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        allTypes = Resources.LoadAll("Items", typeof(ItemType));
        Debug.Log(allTypes);
    }

    public void AddItem()
    {

    }

    public void RemoveItem()
    {

    }
}
