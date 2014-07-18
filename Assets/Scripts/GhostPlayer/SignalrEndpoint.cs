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
        var ghostPositions = GhostPosition.FromGroupString(messageString);
        PeerTracker.Instance.UpdateGhostPositions(ghostPositions);
/*

        var ghostPositions = JsonConvert.DeserializeObject<List<GhostPosition>>(messageString);
        PeerTracker.Instance.UpdateGhostPositions(ghostPositions);
*/

    }

    public void SendPositionUpdate(Vector3 position)
    {
        //var positionString = position.ToString();
        var positionString = position.ToString().Trim('(').Trim(')');
        Application.ExternalCall("$.updatePosition", new[] { positionString });
    }
}
