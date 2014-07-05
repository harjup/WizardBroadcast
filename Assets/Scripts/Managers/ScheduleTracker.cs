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
    public enum EventTarget
    {
        Undefined
    }

    /// <summary>
    /// Holds a list of ScheduledEvents, checks if an event is active, and fires it if it is
    /// </summary>
    class ScheduleTracker : Singleton<ScheduleTracker>
    {
        private Scene activeScene;
        public delegate void ActivateLevel(Scene targetScene);
        public static event ActivateLevel levelActivated;

        private static void OnLevelActivated(Scene targetscene)
        {
            ActivateLevel handler = levelActivated;
            if (handler != null) handler(targetscene);
        }


        //These ScheduledEvents will probably be stored somewhere else eventually
        public List<LevelEvent> Schedule = new List<LevelEvent>()
        {
            new LevelEvent(5f, Scene.Level1),
            new LevelEvent(12f, Scene.Level2),
            new LevelEvent(19f, Scene.Level3),
            new LevelEvent(25f, Scene.Level4),
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
                    //Debug.Log(String.Format("At {0}/{1}, {2}:{3}", scheduledEvent.TargetTime, TimeTracker.Instance.GetCurrentTime().Minute, scheduledEvent.Target, scheduledEvent.Action));
                    OnLevelActivated(scheduledEvent.Target);
                    scheduledEvent.Fired = true;
                }
            }
        }

    }
}
