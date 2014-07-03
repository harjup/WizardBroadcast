using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Assets.Scripts.Pocos;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Holds a list of ScheduledEvents, checks if an event is active, and fires it if it is
    /// </summary>
    class ScheduleTracker : Singleton<ScheduleTracker>
    {
        //TODO: Figure out why events are not firing right now that ScheduleTracker is getting made with bootstrapper
        //These ScheduledEvents will probably be stored somewhere else eventually
        public List<ScheduledEvent> Schedule = new List<ScheduledEvent>()
        {
            new ScheduledEvent(15f, "Game", "Start"),
            new ScheduledEvent(30f, "Wizard", "Fire"),
            new ScheduledEvent(45f, "Level1", "Open"),
            new ScheduledEvent(60f, "Level1", "Close")
        };

        void Start()
        {
          
        }

        void Update()
        {
           CheckSchedule();
        }


        void CheckSchedule()
        {
            foreach (var scheduledEvent in Schedule)
            {
                if (scheduledEvent.IsActive())
                {
                    Debug.Log(String.Format("At {0}/{1}, {2}:{3}", scheduledEvent.TargetTime, TimeTracker.Instance.GetCurrentTime().Minute, scheduledEvent.Target, scheduledEvent.Action));
                    scheduledEvent.Fired = true;
                }
            }
        }

    }
}
