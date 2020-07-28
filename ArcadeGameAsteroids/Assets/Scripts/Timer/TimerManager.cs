using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "TimerManager", menuName = "Managers/TimerManager")]
    public class TimerManager : ManagerBase, IAwake
    {
        public static List<Timer> timers = new List<Timer>();

        public Timer CreateTimer(float waitTime, Timer.OnTimerFinished onTimerFinished = null, Timer.OnTimerTicked onTimerTicked = null)
        {
            timers.Add(new Timer(waitTime, onTimerFinished, onTimerTicked));

            return timers[timers.Count - 1];
        }

        public void DeleteTimer(Timer timer)
        {
            timers.Remove(timer);
        }

        public void Update()
        {
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i].UpdateTimer();
            }
        }

        public void OnAwake()
        {
            GameObject.Find("[Sys_Core]").AddComponent<TimerUpdater>().Setup(this);
            Debug.Log("TimerManager active");
        }

    }
}
