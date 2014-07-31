using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameState;
using Assets.Scripts.Repository;
using UnityEngine;
using WizardCentralServer.Model.Dtos;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Managers
{
    class TweetsManager : Singleton<TweetsManager>
    {

        public int TweetCount = 6;
        private ITweetRepository tweetRepository;
        List<TwitterStatus> statuses;
        private bool hasStatuses = false;

        private GameObject _tweetMessagePrefab;

        void Awake()
        {
            _tweetMessagePrefab = Resources.Load("Prefabs/TweetMessage") as GameObject;

            tweetRepository = new TweetRepository();
            Init();
        }

        void OnLevelWasLoaded(int level)
        {
            Init();
        }

        void Init()
        {
            if (SceneMap.GetSceneFromStringName(Application.loadedLevelName) == Scene.Hub)
            {
                RefreshStatuses();
            }
        }


        void RefreshStatuses()
        {
            StartCoroutine(tweetRepository.GetTweets(x =>
            {
                statuses = x;
                hasStatuses = true;
                PopulateTweets();
            }));
        }

        private void PopulateTweets()
        {
            var statusesToDisplay = statuses;
            var spawners = GetComponentsInChildren<TweetSpawner>().ToList();

            if (statusesToDisplay.Count < TweetCount)    TweetCount = statusesToDisplay.Count;
            if (spawners.Count < TweetCount)             TweetCount = spawners.Count;

            var spawnerSet = new Dictionary<TweetSpawner, TwitterStatus>();

            for (int i = 0; i < TweetCount; i++)
            {
                var index = Random.Range(0, spawners.Count);
                spawnerSet.Add(spawners[index], statusesToDisplay[index]);
                spawners[index].gameObject.SetActive(false);
                spawners.RemoveAt(index);
                statusesToDisplay.RemoveAt(index);
            }

            foreach (var keyValuePair in spawnerSet)
            {
                var spawnedMessage = Instantiate(_tweetMessagePrefab, keyValuePair.Key.transform.position, Quaternion.identity) as GameObject;
                if (spawnedMessage != null) spawnedMessage.GetComponent<TweetMessage>().Status = keyValuePair.Value;
            }

            foreach (var tweetSpawner in spawners)
            {
                tweetSpawner.gameObject.SetActive(false);
            }
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
