using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Utilities.Extensions;

public class Composer : EnsuredSingleton<Composer>
{
    private Object[] itemTypes;
    private Dictionary<string, int> itemDictionary = new Dictionary<string, int>();

    private int currentLevel = 1;

    private int giveReceiveRatio = 0;

    public float[] minutesToCompleteRange;

    private int maxTypeNumber = 3;


    private int difficulty;

    private void Awake()
    {
        SetInitialReferences();
    }

    private void SetInitialReferences()
    {
        itemTypes = Resources.LoadAll("Items", typeof(ItemType));
    }

    public void ComposeNextLevel()
    {
        UpdateItemDictionary();
        difficulty = currentLevel + 2;
        if (giveReceiveRatio > 1) // Allow Requests
        {
            if(Random.value < 0.5)
            {
                CreateGiveEvent();
            }
            else
            {
                CreateRequestEvent();
            }
        }
        else
        {
            CreateGiveEvent();
        }

        currentLevel++;
    }

    private KeyValuePair<string[], int[]> TryToFindGroupToMatchDifficulty(int currentDifficulty)
    {
        string[] largestKeys = { "", "", "" };
        int[] largestValues = { 0, 0, 0 };
        foreach (KeyValuePair<string, int> possibleDrop in itemDictionary)
        {
            for(int i = 0; i < largestValues.Length; i++)
            {
                if(possibleDrop.Value > largestValues[i])
                {
                    largestValues[i] = possibleDrop.Value;
                    largestKeys[i] = possibleDrop.Key;
                }
            }
        }


        return new KeyValuePair<string[], int[]>(largestKeys, largestValues);
    }

    private ItemType GetItemType(string type)
    {
        foreach(ItemType item in itemTypes)
        {
            if(item.type == type)
            {
                return item;
            }
        }
        return null;
    }

    private Dictionary<ItemType, int> GetItemDictionary()
    {
        int difficultyCounter = difficulty - 2;
        Dictionary<ItemType, int> requestDictionary = new Dictionary<ItemType, int>();

        KeyValuePair<string[], int[]> drops = TryToFindGroupToMatchDifficulty(difficultyCounter);

        for(int i = 0; i < drops.Value.Length; i++)
        {
            for (int j = 0; j < drops.Value[j]; j++)
            {
                ItemType type = GetItemType(drops.Key[j]);
                if(requestDictionary.ContainsKey(type))
                {
                    requestDictionary[type]++;
                }
                else
                {
                    requestDictionary.Add(type, 1);
                }

                if (--difficultyCounter <= 0) { break; }
            }
        }

        return requestDictionary;
    }


    private List<int> GetSplit(int numberToSplit)
    {
        int counter = numberToSplit;
        List<int> splits = new List<int>();
        splits.Add(1);
        counter--;
        
        while(counter > 0)
        {
            if(splits.Count >= 3)
            {
                splits[Random.Range(0, splits.Count-1)]++;
                counter--;
                continue;
            }

            if(Random.value < 0.2) // Split
            {
                splits.Add(1);
            }
            else
            {
                splits[Random.Range(0, splits.Count - 1)]++;
            }
            counter--;
        }
        return splits;
    }

    private void CreateGiveEvent()
    {
        List<int> itemSplits = GetSplit(difficulty);
        List<ItemType> items = new List<ItemType>();

        List<ItemType> availableTypes = new List<ItemType>();
        foreach(object itemType in itemTypes)
        {
            availableTypes.Add((ItemType)itemType);
        }
        for(int i = 0; i < itemSplits.Count; i++)
        {
            int index = Random.Range(0, availableTypes.Count);
            items.Add(availableTypes[index]);
            availableTypes.RemoveAt(index);
        }

        ItemEvent itemEvent = new ItemEvent();
        itemEvent.eventType = ItemEventType.AddEvent;
        itemEvent.items = items;
        itemEvent.counts = itemSplits;
        itemEvent.mutationChance = 5;
        itemEvent.timeToExecute = 0;                

        float minutesToExecute = 0.25f;             // TODO: Make Random
        ScheduleItem scheduleItem = new ScheduleItem();
        scheduleItem.itemEvent = itemEvent;
        scheduleItem.minuteToExecute = minutesToExecute;

        GameManager.Instance.AddScheduledEvent(scheduleItem);
        giveReceiveRatio++;
    }

    private void CreateRequestEvent()
    {
        Dictionary<ItemType, int> requestDictionary = GetItemDictionary();
        List<ItemType> items = new List<ItemType>();
        List<int> counts = new List<int>();
        foreach(KeyValuePair<ItemType, int> valuePair in requestDictionary)
        {
            items.Add(valuePair.Key);
            counts.Add(valuePair.Value);
        }


        ItemEvent itemEvent = new ItemEvent();
        itemEvent.eventType = ItemEventType.RemoveEvent;
        itemEvent.items = items;
        itemEvent.counts = counts;
        itemEvent.mutationChance = 5;
        itemEvent.timeToExecute = 30; // TODO: Make Random

        float minutesToExecute = 0.25f; // TODO: Make Random
        ScheduleItem scheduleItem = new ScheduleItem();
        scheduleItem.itemEvent = itemEvent;
        scheduleItem.minuteToExecute = minutesToExecute;

        GameManager.Instance.AddScheduledEvent(scheduleItem);
        giveReceiveRatio--;
    }

    private void UpdateItemDictionary()
    {
        itemDictionary.Clear();
        List<Item> itemList = ItemManager.Instance.globalItems;
        for(int i = 0; i < itemList.Count; i++)
        {
            if(itemDictionary.ContainsKey(itemList[i].type))
            {
                itemDictionary[itemList[i].type]++;
            }
            else
            {
                itemDictionary.Add(itemList[i].type, 1);
            }
        }
    }
}
