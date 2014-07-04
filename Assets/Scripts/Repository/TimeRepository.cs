using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Pocos;
using Newtonsoft.Json;
using UnityEngine;
using WizardCentralServer.Model.Dtos;

namespace Assets.Scripts.Repository
{
    interface ITimeRepository
    {
        IEnumerator GetCurrentTime(Action<DateTime> resultAction);
    }

    class TimeRepository : ITimeRepository
    {
        //TODO: Put this url in a config file.
        private const string TimeUrl = "http://wizardcentralserver.cloudapp.net/api/time";

        public IEnumerator GetCurrentTime(Action<DateTime> resultAction)
        {
            var www = new WWW(TimeUrl);
            yield return www;
            var timedate = JsonConvert.DeserializeObject<TimeApiDate>(www.text);
            Debug.Log("The timeapi time is " + timedate.dateString);
            DateTime currentTime = DateTime.Parse(timedate.dateString);
            resultAction(currentTime);
        }
    }

    class MockTimeRepository : ITimeRepository
    {
        public IEnumerator GetCurrentTime(Action<DateTime> resultAction)
        {
            resultAction(DateTime.Now);
            yield return null;
        }
    }
}
