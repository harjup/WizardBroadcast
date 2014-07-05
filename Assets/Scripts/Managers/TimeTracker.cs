﻿using System;
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
        private const float MinuteInSeconds = 1f;


        private bool _initialized;

        void Start()
        {
            _timeRepository = new MockTimeRepository();
            StartCoroutine(_timeRepository.GetCurrentTime(x =>
            {
                _currentTime = x;

                //For now let's just have the startTime be the beginning of the closest half-hour
                _startTime = new DateTime(_currentTime.Year, _currentTime.Month,
                            _currentTime.Day, _currentTime.Hour, (_currentTime.Minute / 30) * 30, 0);

                _nextMinute = Time.realtimeSinceStartup + (MinuteInSeconds - _currentTime.Second);
                var minuteSpan = (_currentTime.Subtract(_startTime));
                LevelEvent.elapsedMinutes = (float) minuteSpan.TotalMinutes;

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

            LevelEvent.elapsedMinutes += 1;
            if (LevelEvent.elapsedMinutes >= 30f)
            {
                LevelEvent.elapsedMinutes = 0;
                ScheduleTracker.Instance.ResetSchedule();
            }

            
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
