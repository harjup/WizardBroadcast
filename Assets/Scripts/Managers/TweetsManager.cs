using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Repository;
using Newtonsoft.Json;
using UnityEngine;
using WizardCentralServer.Model.Dtos;

namespace Assets.Scripts.Managers
{
    class TweetsManager : Singleton<TweetsManager>
    {

        private ITweetRepository tweetRepository;
        List<TwitterStatus> statuses;

        void Start()
        {
            tweetRepository = new TweetRepository();
            //GetStatuses();
        }

        void GetStatuses()
        {
            StartCoroutine(tweetRepository.GetTweets(x => statuses = x));
        }
    }
}
