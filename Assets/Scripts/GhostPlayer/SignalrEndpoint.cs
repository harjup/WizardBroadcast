using System.Collections;
using System.Runtime.InteropServices;
using Assets.Scripts.Managers;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections.Generic;

public class SignalrEndpoint : Singleton<SignalrEndpoint>
{
    void Awake()
    {
        //Workaround for instantiating a prefrab shoving "(Clone)" at the end
        //Our webpage needs to find this by name
        gameObject.name = "SignalrEndpoint";
    }

    void Start()
    {
        GetPlayerId();
    }

    public void StartGhost()
    {
        Application.ExternalCall("$.startGhost");
    }
    public void StopGhost()
    {
        Application.ExternalCall("$.stopGhost");
    }

    public void GetPlayerId()
    {
        Application.ExternalCall("$.getPlayerId");
    }

    public void Broadcast(string message)
    {
        Application.ExternalCall("$.broadcast", message);
    }

    public void SetPlayerId(string id)
    {
        if (id == "")
        {
            Invoke("GetPlayerId", 1f);
        }

        SessionStateStore.PlayerId = id;
    }

    public void OnRecieveGhostPositions(string messageString)
    {
        var ghostPositions = GhostPosition.FromGroupString(messageString);
        PeerTracker.Instance.UpdateGhostPositions(ghostPositions);

        /*
        var ghostPositions = JsonConvert.DeserializeObject<List<GhostPosition>>(messageString);
        PeerTracker.Instance.UpdateGhostPositions(ghostPositions);
        */

    }

    public void OnRecieveNotification(string message)
    {
        NotificationTextDisplay.Instance.ShowNotification(message);
    }

    public void SendPositionUpdate(Vector3 position)
    {
        //var positionString = position.ToString();
        var positionString = position.ToString().Trim('(').Trim(')');
        Application.ExternalCall("$.updatePosition", new[] { positionString });
    }

    /*
    void OnGUI()
    {
        if (GUI.Button(new Rect(16f, Screen.height - (16 + 32), 32, 32), "Score Mock"))
        {
            OnRecieveScore("myname|10");
            OnRecieveScore("mbuhhe|20");
            OnRecieveScore("asdasdme|5");
        }
        /*
        if (GUI.Button(new Rect(16f, Screen.height - (16 + 32), 32, 32), "GhostMock"))
        {
            OnRecieveGhostPositions("1,2,2,3|2,1,2,5");
        }
        if (GUI.Button(new Rect(16f, Screen.height - ((16 + 32)*2), 32, 32), "Fartmock"))
        {
            OnRecieveNotification("f1|So and So farted >:U");
        }
        if (GUI.Button(new Rect(16f+32, Screen.height - ((16 + 32) * 2), 32, 32), "Fartmock"))
        {
            OnRecieveNotification("f2|So and So farted >:U");
        }#1#
    }*/

    public void SubmitScore(int score)
    {
        Application.ExternalCall("$.submitScore", new[] { GuiManager.Instance.PlayerNameInput, score.ToString() });
    }

    public void OnRecieveScore(string scoreMsg)
    {
        UserProgressStore.Instance.GetSubmittedScore(scoreMsg);
    }

    

}
