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

    public int maxChallengeRating = 25;

    private int giveReceiveRatio = 0;

    public float[] minutesToCompleteRange;

    private int maxTypeNumber = 3;

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
        int difficulty = currentLevel <= maxChallengeRating ? maxChallengeRating : currentLevel;
        UpdateItemDictionary();
        if(giveReceiveRatio > 1) // Allow Requests
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

    private Dictionary<ItemType, int> GetItemDictionary()
    {
        Dictionary<ItemType, int> requestDictionary = new Dictionary<ItemType, int>();



        return requestDictionary;
    }

    private KeyValuePair<List<ItemType>, List<int>> GetSingleItem()
    {
        ItemType availableType = new ItemType();
        for(int i = 0; i < itemTypes.Length; i++)
        {
            ItemType currentType = (ItemType)itemTypes[i];
            if (itemDictionary.ContainsKey(currentType.type))
            {
                availableType = currentType;
            }
        }
        List<ItemType> list = new List<ItemType>();
        list.Add(availableType);
        List<int> list2 = new List<int>();
        list2.Add(1);

        KeyValuePair<List<ItemType>, List<int>> result = new KeyValuePair<List<ItemType>, List<int>>(list, list2);
        return result;
    }


    private List<int> GetSplit(int numberToSplit)
    {
        int counter = numberToSplit;
        List<int> splits = new List<int>();
        
        while(counter > 0)
        {
            if(splits.Count >= 3)
            {
                splits[Random.Range(0, splits.Count-1)]++;
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
        }
        return splits;
    }

    private void CreateGiveEvent()
    {
        int testDifficulty = 10;                            // TODO: Make Random

        List<int> itemSplits = GetSplit(testDifficulty);
        List<ItemType> items = new List<ItemType>();
        for(int i = 0; i < itemSplits.Count; i++)
        {
            items.Add((ItemType)itemTypes[Random.Range(0, itemTypes.Length)]);
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
        int testDifficulty = 10; // TODO: Make Random

        KeyValuePair< List<ItemType>, List<int>> pair = GetSingleItem();

        ItemEvent itemEvent = new ItemEvent();
        itemEvent.eventType = ItemEventType.RemoveEvent;
        itemEvent.items = pair.Key;
        itemEvent.counts = pair.Value;
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
