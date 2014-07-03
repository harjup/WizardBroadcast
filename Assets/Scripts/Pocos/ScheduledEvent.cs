using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Pocos
{
    /// <summary>
    /// Datastructure that will hold all our scheduling bullshit.
    /// </summary>
    public class ScheduledEvent
    {
        public static float elapsedMinutes = 0;

        //Everything is strings for now but that's subject to change
        public ScheduledEvent(float _time, string _target, string _action, string _params = "")
        {
            TargetTime = _time;
            Target = _target;
            Action = _action;
            Params = _params;
            Fired = false;
        }

        /// <summary>
        /// Time the event will fire, in minutes, after session start
        /// </summary>
        public readonly float TargetTime;

        /// <summary>
        /// String representation of the subscriber type the action will fire on
        /// </summary>
        public readonly string Target;

        /// <summary>
        /// Action to envoke on subscribers
        /// </summary>
        public readonly string Action;

        /// <summary>
        /// Parameters to envoke action with, if any
        /// </summary>
        public readonly string Params;

        public bool Fired;

        public bool IsActive()
        {
            return (!Fired && TargetTime <= elapsedMinutes);
        }
    }
}
