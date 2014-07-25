using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Common;

using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Repository
{


    public interface ICommentRepository
    {
        /// <summary>
        /// Gets a load of comments from the world wide web
        /// </summary>
        /// <param name="resultAction">Callback passing in the found comments</param>
        /// <returns></returns>
        IEnumerator GetComments(string levelName, Action<List<UserComment>> resultAction);

        IEnumerator GetComments(string levelname, int time, Action<List<UserComment>> resultAction);

        /// <summary>
        /// Posts a comment to the web
        /// </summary>
        /// /// <param name="comment">Comment to post</param>
        /// <param name="resultAction">Calls a function with a success or failure parameter</param>
        /// <returns></returns>
        IEnumerator PostComment(UserComment comment, Action<bool> resultAction);
    }

    public class CommentRepository : ICommentRepository
    {
        //TODO: Put this in a config file
        //private const string CommentUrl = "http://wizardcentralserver.cloudapp.net/api/comments";
        private const string CommentUrl = "http://localhost:52542/api/comments";

        public IEnumerator GetComments(Action<List<UserComment>> resultAction)
        {
            var commentWWW = new WWW(CommentUrl);
            yield return commentWWW;
            var commentList = JsonConvert.DeserializeObject<List<UserComment>>(commentWWW.text);
            resultAction(commentList);
        }

        public IEnumerator GetComments(string levelName, Action<List<UserComment>> resultAction)
        {
            yield return GetComments(levelName, -1, resultAction);
        }

        public IEnumerator GetComments(string levelname, int time, Action<List<UserComment>> resultAction)
        {
            var url = String.Format("{0}/{1}/{2}",  CommentUrl, levelname, time);
            var commentWWW = new WWW(url);
            yield return commentWWW;
            var commentList = JsonConvert.DeserializeObject<List<UserComment>>(commentWWW.text);
            resultAction(commentList);
        }

        public IEnumerator PostComment(UserComment comment, Action<bool> resultAction)
        {
            var jsonString = JsonConvert.SerializeObject(comment);

            var headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "text/json");

            var encoding = new UTF8Encoding();
            var request = new WWW(CommentUrl, encoding.GetBytes(jsonString), headers);

            yield return request;

            if (request.error != null)
            {
                Debug.Log("Error:" + request.error);
                resultAction(false);
            }
            else
            {
                Debug.Log("Request Successful");
                Debug.Log(request.text);
                resultAction(true);
            }

        }

    }
}
