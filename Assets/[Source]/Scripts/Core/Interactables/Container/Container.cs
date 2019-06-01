using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Container : MonoBehaviour
{
    public Room room;
    private List<Item> itemsInContainer;

    public bool IsInCorrectRoom(Item item)
    {
        return item.room == room.roomType;
    }

    public float CalculateOrganisation()
    {
        Dictionary<string, int> itemsDictionary = new Dictionary<string, int>();

        for (int i = 0; i < itemsInContainer.Count; i++)
        {
            if(itemsDictionary.ContainsKey(itemsInContainer[i].type))
            {
                itemsDictionary[itemsInContainer[i].type]++;
            }
            else
            {
                itemsDictionary.Add(itemsInContainer[i].type, 1);
            }
        }

        // Only one type in the container. Maximum organisation.
        if(itemsDictionary.Count == 1)
        {
            return 1.0f;
        }

        string mostNumerous = "";
        int highestValue = 0;
        foreach(var kvp in itemsDictionary)
        {
            if(kvp.Value > highestValue)
            {
                highestValue = kvp.Value;
                mostNumerous = kvp.Key;
            }
        }

        return highestValue / itemsInContainer.Count;
    }

    public void AddMeToRoom(Room newRoom)
    {
        room = newRoom;
    }

    public void RemoveMeFromRoom()
    {
        room = Room.defaultRoom;
    }

    public void AddItemToContainer(Item itemToAdd)
    {
        if(itemsInContainer.Contains(itemToAdd))
        {
            return;
        }

        itemToAdd.container = this;
        itemsInContainer.Add(itemToAdd);
    }

    public void RemoveItemFromContainer(Item itemToRemove)
    {
        if(!itemsInContainer.Contains(itemToRemove))
        {
            return;
        }

        itemToRemove.container = null;
        itemsInContainer.Remove(itemToRemove);
    }

    private void OnTriggerEnter(Collider other)
    {
        Item otherItem = other.GetComponent<Item>();
        if(otherItem == null)
        {
            return;
        }

        AddItemToContainer(otherItem);
    }

    private void OnTriggerExit(Collider other)
    {
        Item otherItem = other.GetComponent<Item>();
        if (otherItem == null)
        {
            return;
        }

        RemoveItemFromContainer(otherItem);
    }
}
