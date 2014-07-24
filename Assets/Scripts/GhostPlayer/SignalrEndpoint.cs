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

    void OnGUI()
    {
        if (GUI.Button(new Rect(16, Screen.height - (16 + 32), 96,32), "Fart"))
        {
            Broadcast(GuiManager.Instance.PlayerNameInput + " passed gas");
        }
        if (GUI.Button(new Rect(16, Screen.height - (16 + 128), 96,32), "Ghost Appear"))
        {
            OnRecieveGhostPositions("92030666-c950-4dac-9798-585f3db57b65,-3.9, 1.7, 5.3|");
        }
        if (GUI.Button(new Rect(16, Screen.height - (16 + 64), 96, 32), "Ghost Appear2"))
        {
            OnRecieveGhostPositions("92030666-c950-4dac-9798-585f3db57b65,-3.9, 3.8, 5.3|");
        }
        GUI.Label(new Rect(16, Screen.height - (16 + 96), 96, 32), "My id is " + SessionStateStore.PlayerId);
    }
}
