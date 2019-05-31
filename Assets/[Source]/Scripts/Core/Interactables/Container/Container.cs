﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Container : MonoBehaviour
{
    //Room room;
    private List<Item> itemsInContainer;

    private float organisationPercentage;

    public float CalculateOrganisation()
    {
        organisationPercentage = 0;
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
        
        for(int i = 0; i < itemsDictionary.Count; i++)
        {

        }

        return organisationPercentage;
    }

    public void AddItemToContainer(Item itemToAdd)
    {
        if(itemsInContainer.Contains(itemToAdd))
        {
            return;
        }

        //itemToAdd.isContained = true;
        itemsInContainer.Add(itemToAdd);
    }

    public void RemoveItemFromContainer(Item itemToRemove)
    {
        if(!itemsInContainer.Contains(itemToRemove))
        {
            return;
        }

        //itemToAdd.isContained = false;
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
