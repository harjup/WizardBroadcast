using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using WizardCentralServer.Model.Dtos;

namespace Assets.Scripts.Managers
{
    class TweetsManager : Singleton<TweetsManager>
    {
        //TODO: Put this url in a config file.
        string tweetUrl = "http://localhost:52542/api/tweets";

        void Start()
        {
            StartCoroutine(GetTweets());
        }

        IEnumerator GetTweets()
        {
            var tweetWWW = new WWW(tweetUrl);
            yield return tweetWWW;

            var tweets = JsonConvert.DeserializeObject<List<TwitterStatus>>(tweetWWW.text);

            for (int i = 0; i < tweets.Count || i < 5; i++)
            {
                var twitterStatus = tweets[i];
                Debug.Log(String.Format("{0} says {1}", twitterStatus.User.ScreenName, twitterStatus.Text));
                
            }
        }

    }
}
