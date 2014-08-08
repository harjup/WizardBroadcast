using System.Collections.Generic;
using Assets.Scripts.GameState;
using Assets.Scripts.Pocos;
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
            if (targetscene != Scene.Hub
                && targetscene != Scene.Start)
            {
                NotificationTextDisplay.Instance.ShowNotification(
                    "The " + SceneMap.DescriptiveName(targetscene) 
                    + " is now " 
                    + SessionStateStore.DescriptiveStateName[targetState] 
                    );
            }

            if (Application.loadedLevelName != SceneMap.GetScene(Scene.Start) &&
                (targetscene == Scene.Level1 || targetscene == Scene.Level2 || targetscene == Scene.Level3)
                && targetState == State.InActive)
            {
                SignalrEndpoint.Instance.SubmitScore(UserProgressStore.Instance.TreasureTotal());
            }

            
            SessionStateStore.SetSceneState(targetscene, targetState);
            ActivateLevel handler = levelActivated;
            if (handler != null) handler(targetscene, targetState);
        }


        //These ScheduledEvents will probably be stored somewhere else eventually
        public List<LevelEvent> Schedule = new List<LevelEvent>()
        {
            /*new LevelEvent(0f, Scene.Hub, State.Active),
            new LevelEvent(0f, Scene.Level1, State.Active),
            new LevelEvent(0f, Scene.Level2, State.Active),
            new LevelEvent(0f, Scene.Level3, State.Active),
            new LevelEvent(0f, Scene.Level4, State.Active),*/

            /*new LevelEvent(0f, Scene.Hub, State.Active),
            new LevelEvent(1f, Scene.Level1, State.Active),
            new LevelEvent(9f, Scene.Level1, State.InActive),
            new LevelEvent(10f, Scene.Level2, State.Active),
            new LevelEvent(17f, Scene.Level2, State.InActive),
            new LevelEvent(18f, Scene.Level3, State.Active),
            new LevelEvent(25f, Scene.Level3, State.InActive),
            new LevelEvent(26f, Scene.Hub, State.InActive),*/    

            new LevelEvent(0f, Scene.Hub, State.Active),
            new LevelEvent(1f, Scene.Level1, State.Active),
            new LevelEvent(5f, Scene.Level1, State.InActive),
            new LevelEvent(5f, Scene.Level2, State.Active),
            new LevelEvent(9f, Scene.Level2, State.InActive),
            new LevelEvent(9f, Scene.Level3, State.Active),
            new LevelEvent(13f, Scene.Level3, State.InActive),
            new LevelEvent(14f, Scene.Hub, State.InActive)
        };

        void Start()
        {

        }

        void Update()
        {
            //Don't start checking the schedule until the timetracker has gotten the current time
            if (TimeTracker.Instance.IsInitialized())
            {
                CheckSchedule();
                SessionStateStore.SetScheduleTrackInit();
            }
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

                if (scheduledEvent.WillFireSoon())
                {
                    NotificationTextDisplay.Instance.ShowNotification(
                   "The " + SceneMap.DescriptiveName(scheduledEvent.Target)
                   + " will be "
                   + SessionStateStore.DescriptiveStateName[scheduledEvent.TargetState]
                   + " in 1 minute!"
                   );

                    scheduledEvent.ReminderFired = true;
                }
            }
        }

        public float NextAirTime()
        {
            foreach (var scheduledEvent in Schedule)
            {
                if (scheduledEvent.Target == Scene.Hub && scheduledEvent.TargetState == State.Active)
                {
                    var timeDifference = scheduledEvent.TargetTime - TimeTracker.Instance.GetSessionTime();
                    return timeDifference <= 0 ? 30 + timeDifference : timeDifference;
                }
            }
            return 0f;
        }
    }
}
