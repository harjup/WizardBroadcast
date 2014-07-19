using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Utilities;

public class RoomWorkflow : MonoBehaviourBase
{
    public PuzzleRoomManager[] RoomManagers;
    private int currentRoomIndex = 0;


    void Awake()
    {
        RoomManagers = GetComponentsInChildren<PuzzleRoomManager>();
        currentRoomIndex = 0;
    }

    PuzzleRoomManager NextRoom()
    {
        if (currentRoomIndex < RoomManagers.Length - 1)
        {
            currentRoomIndex++;
        }
        return CurrentRoom;
    }

    public PuzzleRoomManager CurrentRoom
    {
        get { return RoomManagers[currentRoomIndex]; }
    }
}

