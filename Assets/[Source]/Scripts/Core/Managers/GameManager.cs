using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;

public class GameManager : MonoBehaviour
{
    [OdinSerialize]
    public List<ScheduleItem> schedule = new List<ScheduleItem>();
    public float secondsPerTick;
    float timer;
    float globalTimer;
    int schedulePos = 0;

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
        if (schedule.Count <= schedulePos)
        {
            if ((schedule[schedulePos].minuteToExecute * 60) <= globalTimer)
            {
                StartCoroutine(schedule[schedulePos++].itemEvent.HandleEvent());
            }
        }
    }

    private void Tick()
    {
        ItemManager.Instance.CallEventDegradeCallback();
    }


}
