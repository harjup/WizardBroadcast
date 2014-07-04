using System;
using System.Collections;
using Assets.Scripts.Pocos;
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
        //TODO: Put this url in a config file.
        private const string TimeUrl = "http://localhost:52542/api/time";

        private float _nextMinute;
        private static DateTime _startTime;
        static DateTime _currentTime;
        private const float MinuteInSeconds = 60f;


        private bool initialized = false;

        void Start()
        {
            StartCoroutine(GetStartTime());
        }

        void Update()
        {
            if (initialized)
            {
                CheckTime();
            }
            
        }

        IEnumerator GetStartTime()
        {
            _startTime = DateTime.Now.AddMinutes(-5);
            //_currentTime = DateTime.Now;

            var www = new WWW(TimeUrl);
            yield return www;
            var timedate = JsonConvert.DeserializeObject<TimeApiDate>(www.text);
            Debug.Log("The timeapi time is " + timedate.dateString);
            _currentTime = DateTime.Parse(timedate.dateString);

            _nextMinute = Time.realtimeSinceStartup + (MinuteInSeconds - _currentTime.Second);

            var minutesSpan = (_currentTime.Subtract(_startTime));
            ScheduledEvent.elapsedMinutes = (float)minutesSpan.TotalMinutes;

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
