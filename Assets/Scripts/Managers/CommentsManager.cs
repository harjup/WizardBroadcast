using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Common;
using Assets.Scripts.GameState;
using Assets.Scripts.Pocos;
using Assets.Scripts.Repository;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Gets user comments from the webapi, wow!!!!
    /// </summary>
    class CommentsManager : Singleton<CommentsManager>
    {
        private ICommentRepository commentRepository;
        private List<UserComment> userComments;
        private GameObject _messengerPrefab;

        void Start()
        {
        }


        void Awake()
        {
            _messengerPrefab = Resources.Load("Prefabs/UserMessenger") as GameObject;
            commentRepository = new CommentRepository();
            Init();
        }

        void OnLevelWasLoaded(int level)
        {
            Init();
        }

        void Init()
        {
            if (SceneMap.GetSceneFromStringName(Application.loadedLevelName) != Scene.Start)
            {
                GetComments();
            }
        }


        void GetComments()
        {
            StartCoroutine(commentRepository.GetComments(
                Application.loadedLevelName,
                TimeTracker.Instance.GetSessionTime(),
                x =>
            {
                userComments = x;
                SpawnMessengers();
            }));
        }


        void SpawnMessengers()
        {
            var container = GameObject.Find("UserMessengerContainer");
            if (container == null)
            {
                container = new GameObject("UserMessengerContainer");
            }

            foreach (var userComment in userComments)
            {
                var messenger = Instantiate(_messengerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                messenger.GetComponent<UserMessenger>().SetComment(userComment);
                messenger.transform.parent = container.transform;
                messenger.name = userComment.Name + userComment.ID;
            }
        }

        public void PostComment(UserComment commentToPost)
        {
            var timetracker = TimeTracker.Instance;

            commentToPost.Location = Application.loadedLevelName;
            commentToPost.SessionTime = timetracker.GetSessionTime();
            commentToPost.DateTime = timetracker.GetCurrentTime();
            StartCoroutine(commentRepository.PostComment(
                    commentToPost, 
                    x => Debug.Log(x ? "Comment Posted" : "Comment Not Posted"))
                );
        }
    }
}
