using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Repository;
using WizardCentralServer.Model.Dtos;

namespace Assets.Scripts.Managers
{
    class TweetsManager : Singleton<TweetsManager>
    {

        private ITweetRepository tweetRepository;
        List<TwitterStatus> statuses;
        private bool hasStatuses = false;

        void Awake()
        {
            tweetRepository = new TweetRepository();
            GetStatuses();
        }

        void GetStatuses()
        {
            StartCoroutine(tweetRepository.GetTweets(x =>
            {
                statuses = x;
                hasStatuses = true;
            }));
        }

        public void GetFirstStatus(Action<TwitterStatus> callback)
        {
            if (!hasStatuses)
            {
                StartCoroutine(tweetRepository.GetTweets(x =>
                {
                    statuses = x;
                    hasStatuses = true;
                    callback(statuses.First());
                }));
            }
            else
            {
                callback(statuses.First());
            }
        }
    }
}
