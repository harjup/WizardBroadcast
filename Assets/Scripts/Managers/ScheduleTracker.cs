using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Assets.Scripts.Pocos;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using WizardBroadcast;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Holds a list of ScheduledEvents, checks if an event is active, and fires it if it is
    /// </summary>
    class ScheduleTracker : Singleton<ScheduleTracker>
    {
        private SceneMap.scene activeScene;


        //These ScheduledEvents will probably be stored somewhere else eventually
        public List<ScheduledEvent> Schedule = new List<ScheduledEvent>()
        {
            new ScheduledEvent(2f, "Game", "Start"),
            new ScheduledEvent(12f, "Wizard", "Fire"),
            new ScheduledEvent(15f, "Level1", "Open"),
            new ScheduledEvent(17f, "Level1", "Close")
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
