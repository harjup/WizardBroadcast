using UnityEngine;
using System.Collections.Generic;

public class HubRoomWorkflow : MonoBehaviourBase
{
    private RoomManager Outside;
    private List<RoomManager> Floors = new List<RoomManager>();

    RoomManager GetFloor(int index)
    {
        return Floors[index - 1];
    }
}
