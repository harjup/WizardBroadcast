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
        private Scene activeScene;
        public delegate void ActivateLevel(Scene targetScene, State targetState);
        public static event ActivateLevel levelActivated;

        private static void OnLevelActivated(Scene targetscene, State targetState)
        {
            SessionStateStore.SetSceneState(targetscene, targetState);
            ActivateLevel handler = levelActivated;
            if (handler != null) handler(targetscene, targetState);
        }


        //These ScheduledEvents will probably be stored somewhere else eventually
        public List<LevelEvent> Schedule = new List<LevelEvent>()
        {
            new LevelEvent(5f, Scene.Level1, State.Active),
            new LevelEvent(11f, Scene.Level1, State.InActive),
            new LevelEvent(12f, Scene.Level2, State.Active),
            new LevelEvent(17f, Scene.Level2, State.InActive),
            new LevelEvent(19f, Scene.Level3, State.Active),
            new LevelEvent(23f, Scene.Level3, State.InActive),
            new LevelEvent(25f, Scene.Level4, State.Active),
            new LevelEvent(29f, Scene.Level4, State.InActive)
        };

        void Start()
        {

        }

        void Update()
        {
           CheckSchedule();
        }

        public void ResetSchedule()
        {
            foreach (var scheduledEvent in Schedule)
            {
                scheduledEvent.Fired = false;
            }
        }

        void CheckSchedule()
        {
            foreach (var scheduledEvent in Schedule)
            {
                if (scheduledEvent.IsActive())
                {
                    OnLevelActivated(scheduledEvent.Target, scheduledEvent.TargetState);
                    scheduledEvent.Fired = true;
                }
            }
        }

    }
}
