using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Utilities.Extensions;

public class ItemSpawner : EnsuredSingleton<ItemSpawner>
{
    public GameObject SpawnItem(GameObject item, Vector3 position)
    {
        GameObject newItem = Instantiate(item, transform);
        newItem.transform.position = position;
        return newItem;
    }
}
