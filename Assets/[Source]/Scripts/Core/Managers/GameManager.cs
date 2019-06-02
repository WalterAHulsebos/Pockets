using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Utilities.Extensions;
using Utilities;

public class GameManager : EnsuredSingleton<GameManager>
{
    public GameObject ratPrefab;
    public List<Transform> ratSpawnLocations = new List<Transform>();

    [Header("Schedule")]
    public List<ScheduleItem> schedule = new List<ScheduleItem>();
    public float secondsPerTick;
    float timer;
    float globalTimer;
    int schedulePos = 0;

    private ItemEvent currentRequestEvent;

    private void Start()
    {
        SetInitialReferences();
    }

    private void SetInitialReferences()
    {
        Composer.Instance.ComposeNextLevel();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        globalTimer += Time.deltaTime;

        if(timer >= secondsPerTick)
        {
            timer = 0;
            Tick();
        }

        CheckSchedule();
    }

    public void CheckSchedule()
    {
        if (schedule.Count > 0)
        {
            if ((schedule[0].minuteToExecute * 60) <= globalTimer)
            {
                if(schedule[0].itemEvent.eventType == ItemEventType.RemoveEvent)
                {
                    currentRequestEvent = schedule[0].itemEvent;
                }
                StartCoroutine(schedule[0].itemEvent.HandleEvent());
                schedule.RemoveAt(0);
                Composer.Instance.ComposeNextLevel();
                globalTimer = 0;
            }
        }
        if (currentRequestEvent != null)
        {
            if (currentRequestEvent.timeToExecute <= 0)
            {
                currentRequestEvent = null;
            }
        }
    }

    private void Tick()
    {
        ItemManager.Instance.CallEventDegradeCallback();
    }

    public void AddScheduledEvent(ScheduleItem scheduledItem)
    {
        schedule.Add(scheduledItem);
    }

    public ItemEvent GetActiveRequestEvent()
    {
        return currentRequestEvent;
    }

    public void SpawnRats(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Instantiate(ratPrefab, ratSpawnLocations[Random.Range(0, ratSpawnLocations.Count - 1)].position, Quaternion.identity);
        }
    }
}
