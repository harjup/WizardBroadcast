using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using WizardCentralServer.Model.Dtos;

namespace Assets.Scripts.Repository
{
    //Let's code against an interface so we can swap out the repo with a mock
    public interface ITweetRepository
    {
        IEnumerator GetTweets(Action<List<TwitterStatus>> resultAction);
    }


    public class TweetRepository : ITweetRepository
    {
        //TODO: Put this url in a config file.
        private const string TweetUrl = "http://wizardcentralserver.cloudapp.net/api/tweets";

        public IEnumerator GetTweets(Action<List<TwitterStatus>> resultAction)
        {
            var tweetWWW = new WWW(TweetUrl);
            yield return tweetWWW;

            var tweets = JsonConvert.DeserializeObject<List<TwitterStatus>>(tweetWWW.text);

            /*for (int i = 0; i < tweets.Count || i < 5; i++)
            {
                var twitterStatus = tweets[i];
                Debug.Log(String.Format("{0} says {1}", twitterStatus.User.ScreenName, twitterStatus.Text));
            }*/

            resultAction(tweets);
        }
    }
}
