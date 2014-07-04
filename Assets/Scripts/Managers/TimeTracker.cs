using System;
using System.Collections;
using Assets.Scripts.Pocos;
using Assets.Scripts.Repository;
using Newtonsoft.Json;
using UnityEngine;
using WizardCentralServer.Model.Dtos;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Keeps track of how many minutes have passed since the start of the game.
    /// Gets the session start some the webapi, probably
    /// </summary>
    class TimeTracker : Singleton<TimeTracker>
    {
        private ITimeRepository _timeRepository;

        private float _nextMinute;
        private static DateTime _startTime;
        static DateTime _currentTime;
        private const float MinuteInSeconds = 60f;


        private bool _initialized;

        void Start()
        {
            _timeRepository = new MockTimeRepository();
            StartCoroutine(_timeRepository.GetCurrentTime(x =>
            {
                _currentTime = x;

                //TODO: Determine how we shoudl be calcualting session starts and ends.
                //Maybe it should just be based on how far into the current hour or 1/2 hour we are
                _startTime = _currentTime.AddMinutes(-5);
                _nextMinute = Time.realtimeSinceStartup + (MinuteInSeconds - _currentTime.Second);
                var minuteSpan = (_currentTime.Subtract(_startTime));
                ScheduledEvent.elapsedMinutes = (float) minuteSpan.TotalMinutes;

                _initialized = true;
            }));
        }

        void Update()
        {
            if (_initialized)
            {
                CheckTime();
            }
            
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
