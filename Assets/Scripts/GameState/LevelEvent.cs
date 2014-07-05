using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Pocos
{
    /// <summary>
    /// Datastructure that will hold all our scheduling bullshit.
    /// </summary>
    public class LevelEvent
    {

        public static float elapsedMinutes = 0;

        //Everything is strings for now but that's subject to change
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

        public bool IsActive()
        {
            return (!Fired && TargetTime <= elapsedMinutes);
        }
    }
}
