using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // Because screw you and your good programming practice.
    public static RoomManager instance;
    private List<Room> rooms = new List<Room>();

    private void Awake()
    {
        SetInitialReferences();
    }

    private void SetInitialReferences()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public float CalculateOverallScore()
    {
        float finalScore = 0.0f;
        foreach(Room room in rooms)
        {
            finalScore += room.CalculateRoomOrganisation();
        }
        return finalScore;
    }

    public void RegisterRoom(Room newRoom)
    {
        if(!rooms.Contains(newRoom))
        {
            rooms.Add(newRoom);
        }
    }

}
