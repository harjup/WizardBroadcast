using UnityEngine;
using System.Collections;

public class MazeExitSet : MonoBehaviourBase
{
    private RoomManager _roomManager;


    private enum Direction
    {
        Undefined,
        North,
        South,
        East,
        West
    };

    private static Direction[] roomDirections =
    {
        Direction.East, 
        Direction.West, 
        Direction.North, 
        Direction.South, 
        Direction.East, 
        Direction.West, 
        Direction.South, 
        Direction.East
    };

    void Start()
    {
        _roomManager = GetComponentInParent<RoomManager>();
        if (_roomManager == null)
        {
            Debug.LogError("Expected exitdoorway to have a room manager parent");
        }

        var myDirection = roomDirections[_roomManager.RoomIndex];

        foreach (var exitDoorway in GetComponentsInChildren<ExitDoorway>())
        {
            if (exitDoorway.name != myDirection.ToString())
            {
                exitDoorway.targetRoomIndex = 0;
            }
        }
    }


    
}
