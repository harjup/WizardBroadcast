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

    public void SetPlayerId(string id)
    {
        SessionStateStore.PlayerId = id;
    }

    public void OnRecieveGhostPositions(string messageString)
    {
        /*
            var ghostPositions = GhostPosition.FromString(messageString);
            PeerTracker.Instance.UpdateGhostPositions(ghostPositions);
        */
        var ghostPositions = JsonConvert.DeserializeObject<List<GhostPosition>>(messageString);
        PeerTracker.Instance.UpdateGhostPositions(ghostPositions);
    }

    public void SendPositionUpdate(Vector3 position)
    {
        var positionString = position.ToString();
        /*
            var positionString = position.ToString().Trim('(').Trim(')');
        */
        Application.ExternalCall("$.updatePosition", new[] { positionString });
    }

   /* void OnGUI()
    {
        if (GUI.Button(new Rect(10,10,100,50), "Test 1"))
        {
            OnRecieveGhostPositions("[{\"name\":\"d2c782dc-16ed-4146-b577-5f047a6f7cf2\",\"position\":\"(1.0, 3.8, -13.2)\"},{\"name\":\"be53f8d2-bc9e-49ff-995b-39fc86bf9ac8\",\"position\":\"(28.8, 2.0, -12.7)\"}]");
        }

        if (GUI.Button(new Rect(120, 10, 100, 50), "Test 2"))
        {
            OnRecieveGhostPositions("[{\"name\":\"d2c782dc-16ed-4146-b577-5f047a6f7cf2\",\"position\":\"(1.0, 3.8, -13.2)\"},{\"name\":\"be53f8d2-bc9e-49ff-995b-39fc86bf9ac8\",\"position\":\"(28.8, 2.0, -6)\"}]");
        }
    }*/
}
