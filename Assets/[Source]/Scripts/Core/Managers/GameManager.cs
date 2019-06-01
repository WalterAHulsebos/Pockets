using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Utilities.Extensions;
using Utilities;

public class GameManager : PersistentSingleton<GameManager>
{
    public List<ScheduleItem> schedule = new List<ScheduleItem>();
    public float secondsPerTick;
    float timer;
    float globalTimer;
    int schedulePos = 0;

    private ItemEvent currentRequestEvent;

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
        if (schedule.Count > schedulePos)
        {
            if ((schedule[schedulePos].minuteToExecute * 60) <= globalTimer)
            {
                if(schedule[schedulePos].itemEvent.eventType == ItemEventType.RemoveEvent)
                {
                    currentRequestEvent = schedule[schedulePos].itemEvent;
                }
                StartCoroutine(schedule[schedulePos++].itemEvent.HandleEvent());
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

    public ItemEvent GetActiveRequestEvent()
    {
        return currentRequestEvent;
    }
}
