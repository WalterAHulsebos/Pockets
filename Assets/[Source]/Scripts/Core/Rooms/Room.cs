using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isDefaultRoom = false;
    public static Room defaultRoom;

    public ItemRoom roomType;
    private List<Container> containersInRoom = new List<Container>();

    private void Start()
    {
        SetInitialReferences();
    }

    private void SetInitialReferences()
    {
        if(isDefaultRoom)
        {
            defaultRoom = this;
        }

        RoomManager.instance.RegisterRoom(this);
    }

    public float CalculateRoomOrganisation()
    {
        // Anything in the default room in not organised.
        if(isDefaultRoom)
        {
            return 0.0f;
        }

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
}
