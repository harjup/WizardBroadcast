using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Gets user comments from the webapi, wow!!!!
    /// </summary>
    class CommentsManager : Singleton<CommentsManager>
    {
        //TODO: Put this url in a config file.
        string commentUrl = "http://localhost:52542/api/comments";

        void Start()
        {
            StartCoroutine(GetComments());
        }


        IEnumerator GetComments()
        {
            var commentWWW = new WWW(commentUrl);
            yield return commentWWW;

            var commentList = JsonConvert.DeserializeObject<List<UserComment>>(commentWWW.text);
            foreach (var userComment in commentList)
            {
                Debug.Log(String.Format("{0} says {1}", userComment.Name, userComment.Content));
                if (userComment.WorldPositon != null)
                {
                    Vector3 position = userComment.WorldPositon.ParseToVector3();
                    Debug.Log(position);
                }

            }
        }

        IEnumerator PostComment()
        {
            
            var commentToSave = new UserComment()
            {
                Content = "Hello from the Unity3D webplayer!!",
                Mood = "Aprehensive",
                Name = "Poopy Pants3D",
                SessionTime = 5,
                Location = "Hub",
                WorldPositon = Vector3.zero.ToString()
            };

            var jsonString = JsonConvert.SerializeObject(commentToSave);

            var headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "text/json");

            var encoding = new UTF8Encoding();
            var request = new WWW(commentUrl, encoding.GetBytes(jsonString), headers);

            yield return request;

            if (request.error != null)
            {
                Debug.Log("Error:" + request.error);
            }
            else
            {
                Debug.Log("Request Successful");
                Debug.Log(request.text);
            }
            
        }

    }
}
