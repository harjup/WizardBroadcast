using System;
using UnityEngine;
using System.Collections;

public class MazeExitSet : MonoBehaviourBase
{
    public int BeginningIndex = 0;

    private RoomManager _roomManager;
    private RoomWorkflow _workflow; //Violating layer bullshit here but ~whatever~

    private enum Direction
    {
        Undefined,
        North,
        South,
        East,
        West
    };

    private static readonly Direction[] RoomDirections =
    {
        Direction.East, 
        Direction.West, 
        Direction.North, 
        Direction.South, 
        Direction.East, 
        Direction.West, 
        Direction.South, 
        Direction.East,
        Direction.Undefined // The last room sends you back no matter the direction
    };

    void Start()
    {
        _workflow = GetComponentInParent<RoomWorkflow>();
        _roomManager = GetComponentInParent<RoomManager>();
        if (_roomManager == null)
        {
            Debug.LogError("Expected exitdoorway to have a room manager parent");
        }

        InitExitSet();
    }

    public void InitExitSet()
    {
        var myDirection = RoomDirections[_roomManager.RoomIndex];

        var beginningIndex = 0;
        if (_workflow != null && _workflow.ReverseRooms)
        {
            myDirection = ReverseDirection(myDirection);
            beginningIndex = _workflow.RoomManagers.Length - 1;
        }

        foreach (var exitDoorway in GetComponentsInChildren<ExitDoorway>())
        {
            if (exitDoorway.name != myDirection.ToString())
            {
                exitDoorway.targetRoomIndex = beginningIndex;
            }
            else
            {
                exitDoorway.targetRoomIndex = -1;
            }
        }

        BeginningIndex = beginningIndex;
    }

    Direction ReverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Undefined:
                break;
            case Direction.North:
                return Direction.South;
            case Direction.South:
                return Direction.North;
            case Direction.East:
                return Direction.West;
            case Direction.West:
                return Direction.East;
            default:
                throw new ArgumentOutOfRangeException("direction");
        }

        return Direction.Undefined;
    }

    public void DisableExits()
    {
        foreach (var exitDoorway in GetComponentsInChildren<ExitDoorway>())
        {
            exitDoorway.Activate(false);
        }
    }
}
