using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;

[System.Serializable]
public enum ItemRoom
{
    Freezer,
    Armory,
    Library,
    Bank,
    Trophy
}

[System.Flags, System.Serializable]
public enum ItemEffects
{
    None = 0x0,         //00000000
    Fire = 0x1,         //00000001    
    Poison = 0x2,       //00000010
    Freeze = 0x4,       //00000100
    //TODO: Add more effects if needed
}

[System.Serializable]
public enum ItemEventType
{
    AddEvent,
    RemoveEvent
}

[System.Serializable]
public class ItemEvent
{
    public ItemEventType eventType;
    public List<Item> items;
    public List<int> counts;
    public float mutationChance;
    
    public int timeToExecute;

    public IEnumerator HandleEvent()
    {
        while(timeToExecute > 0 && eventType == ItemEventType.RemoveEvent)
        {
            timeToExecute -= 1;
            yield return new WaitForSecondsRealtime(1f);
        }

        switch(eventType)
        {
            case ItemEventType.AddEvent:
                //ItemManager.Instance.CreateItems(items, mutationChance);
                Debug.Log("ADD");
                break;

            case ItemEventType.RemoveEvent:
                //ItemManager.Instance.RemoveItems(items);
                Debug.Log("REMOVE");
                break;
        }

    }
}

[System.Serializable]
public class ScheduleItem
{
    public float minuteToExecute;
    public ItemEvent itemEvent;
}

