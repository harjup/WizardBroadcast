using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameState;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Pocos
{
    /// <summary>
    /// Datastructure that will hold all our scheduling bullshit.
    /// </summary>
    public class LevelEvent
    {
        private const float SessionLength = 30f;

        /// <summary>
        /// Amount of time after the session has started.
        /// Automatically resets itself if set above the SessionLength
        /// </summary>
        public static float ElapsedMinutes
        {
            get { return _elapsedMinutes; }
            set
            {
                _elapsedMinutes = value;
                if (ElapsedMinutes >= SessionLength)
                {
                    ElapsedMinutes = 0;
                    //Might be a good idea to just first an event instead of manually calling reset schedule
                    ScheduleTracker.Instance.ResetSchedule();
                }
            }
        }

        public LevelEvent(float _time, Scene _targetScene, State targetState)
        {
            TargetTime = _time;
            Target = _targetScene;
            TargetState = targetState;
            Fired = false;
        }

        /// <summary>
        /// Time the event will fire, in minutes, after session start
        /// </summary>
        public readonly float TargetTime;

        /// <summary>
        /// The target scene to act on
        /// </summary>
        public readonly Scene Target;

        /// <summary>
        /// The state that we're telling the scene to switch to
        /// </summary>
        public readonly State TargetState;

        public bool Fired;
        private static float _elapsedMinutes;

        public bool IsActive()
        {
            return (!Fired && TargetTime <= ElapsedMinutes);
        }
    }
}
