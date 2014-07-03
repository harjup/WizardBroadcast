using System;
using System.Collections;
using Assets.Scripts.Pocos;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Keeps track of how many minutes have passed since the start of the game.
    /// Gets the session start some the webapi, probably
    /// </summary>
    class TimeTracker : Singleton<TimeTracker>
    {
        private float _nextMinute = 0f;
        private static DateTime _startTime;
        static DateTime _currentTime;
        private const float MinuteInSeconds = 2f;

        private bool initialized = false;

        void Start()
        {
            GetStartTime();
            _nextMinute = Time.realtimeSinceStartup + (MinuteInSeconds - _currentTime.Second);
            ScheduledEvent.elapsedMinutes = _currentTime.Minute;
            StartCoroutine(GetStartTime());
        }

        void Update()
        {
            if (initialized)
            {
                CheckTime();
            }
            
        }

        //TODO: Make a trip to a webapi to find out when the session started and what time it is now.
        IEnumerator GetStartTime()
        {
            _startTime = DateTime.Now;
            _currentTime = DateTime.Now;

            initialized = true;
            yield return null;
        }

        void CheckTime()
        {
            var time = Time.realtimeSinceStartup;
            if (time >= _nextMinute)
            {
                IncrementMinute(time);
            }
        }

        void IncrementMinute(float time)
        {
            _currentTime = _currentTime.AddMinutes(1);
            var secondsOver = (time - _nextMinute);
            _nextMinute = time + (MinuteInSeconds - secondsOver);
            ScheduledEvent.elapsedMinutes += 1;
        }

        public DateTime GetCurrentTime()
        {
            return _currentTime;
        }

        void OnGUI()
        {
            GUI.Label(new Rect(Screen.width - 100f, 10f, 200f, 50f), String.Format("{0}:{1}", _currentTime.Hour, _currentTime.Minute));
        }
    }
}
