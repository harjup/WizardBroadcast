using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Common;
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

        void Start()
        {
            commentRepository = new CommentRepository();
            //GetComments();
            //PostComment();
        }

        void GetComments()
        {
            StartCoroutine(commentRepository.GetComments(x => {userComments = x;}));
        }

        void PostComment()
        {
            var comment = new UserComment
            {
                Name = "Unity Player",
                Content = "Hello from wonderful unity three d!!",
                Mood = "Whatever",
                Location = "Hub",
                WorldPositon = Vector3.forward.ToString()
            };

            StartCoroutine(commentRepository.PostComment(comment, x => Debug.Log(x ? "Comment Posted" : "Comment Not Posted")));
        }
    }
}
