using System;
using System.Collections;
using Assets.Scripts.Pocos;
using Assets.Scripts.Repository;
using Newtonsoft.Json;
using UnityEngine;
using WizardCentralServer.Model.Dtos;

namespace Assets.Scripts.Managers
{
    using UnityEngine;
    /// <summary>
    /// Keeps track of how many minutes have passed since the start of the game.
    /// Gets the session start some the webapi, probably
    /// </summary>
    class TimeTracker : Singleton<TimeTracker>
    {
        //Make this this is evenly divisible by an hour so it can loop on the hour ok????
        private const int SessionLength = 30;

        private ITimeRepository _timeRepository;

        private float _nextMinute;
        private static DateTime _startTime;
        static DateTime _currentTime;
        //private const float MinuteInSeconds = 60f;
        private const float MinuteInSeconds = 60f;


        private bool _initialized;

        void Start()
        {
            if (!_initialized)
            {
                _timeRepository = new MockTimeRepository();
                //_timeRepository = new TimeRepository();
                StartCoroutine(_timeRepository.GetCurrentTime(x =>
                {
                    _currentTime = x;

                    //For now let's just have the startTime be the beginning of the closest half-hour
                    _startTime = new DateTime(_currentTime.Year, _currentTime.Month,
                                _currentTime.Day, _currentTime.Hour, (_currentTime.Minute / 30) * 30, 0);

                    _nextMinute = Time.realtimeSinceStartup + (MinuteInSeconds - _currentTime.Second);
                    //var minuteSpan = (_currentTime.Subtract(_startTime));
                    //LevelEvent.ElapsedMinutes = (float)minuteSpan.TotalMinutes;

                    _initialized = true;
                }));
            }
        }

        void Update()
        {
            if (_initialized)
            {
                CheckTime();
            }
            
        }

        public bool IsInitialized()
        {
            return _initialized;
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

            var sessionTime = GetSessionTime();
            //Maybe somewhere else
            if (sessionTime == 0)
            {
                ScheduleTracker.Instance.ResetSchedule();
            }

        }

        public DateTime GetCurrentTime()
        {
            return _currentTime;
        }

        void OnGUI()
        {
            GUI.Label(new Rect(Screen.width - 64f, 16f, 200f, 50f), String.Format("{0}:{1}", _currentTime.Hour.ToString("D2"), _currentTime.Minute.ToString("D2")));
            /*if (GUI.Button(new Rect(Screen.width - 96f, 160f, 64f, 32f), String.Format("Min++")))
            {
                IncrementMinute(0f);
            }*/
        }

        public int GetSessionTime()
        {
            var minutes = _currentTime.Minute;
            while (minutes >= SessionLength)
            {
                minutes -= SessionLength;
            }
            return minutes;
        }
    }
}
