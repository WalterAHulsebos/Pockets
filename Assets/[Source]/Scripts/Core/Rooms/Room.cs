using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isDefaultRoom = false;
    public static Room defaultRoom;

    public ItemRoom roomType;
    private List<Container> containersInRoom = new List<Container>();

    private void Awake()
    {
        SetInitialReferences();
    }

    private void SetInitialReferences()
    {
        if(isDefaultRoom)
        {
            defaultRoom = this;
        }
    }

    public float CalculateRoomOrganisation()
    {
        float overallOrganisation = 0;
        for (int i = 0; i < containersInRoom.Count; i++)
        {
            overallOrganisation += containersInRoom[i].CalculateOrganisation();
        }

        return overallOrganisation;
    }

    public void AddContainerToRoom(Container containerToAdd)
    {
        if (containersInRoom.Contains(containerToAdd))
        {
            return;
        }
        
        containersInRoom.Add(containerToAdd);
    }

    public void RemoveContainerToRoom(Container containerToRemove)
    {
        if (!containersInRoom.Contains(containerToRemove))
        {
            return;
        }
        
        containersInRoom.Remove(containerToRemove);
    }

    private void OnTriggerEnter(Collider other)
    {
        Container otherItem = other.GetComponent<Container>();
        if (otherItem == null)
        {
            return;
        }

        AddContainerToRoom(otherItem);
    }

    private void OnTriggerExit(Collider other)
    {
        Container otherItem = other.GetComponent<Container>();
        if (otherItem == null)
        {
            return;
        }

        RemoveContainerToRoom(otherItem);
    }
}
