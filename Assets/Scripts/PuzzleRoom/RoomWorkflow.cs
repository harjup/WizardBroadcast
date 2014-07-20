using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Utilities;
using UnityEngine;

public class RoomWorkflow : MonoBehaviourBase
{
    public PuzzleRoomManager[] RoomManagers;
    public PuzzleRoomManager FinalRoom;

    [SerializeField]
    private int currentRoomIndex = 0;

    void Awake()
    {
        RoomManagers = GetComponentsInChildren<PuzzleRoomManager>();
        currentRoomIndex = 0;

        for (int i = 0; i < RoomManagers.Length; i++)
        {
            RoomManagers[i].RoomIndex = i;
        }
    }

    public PuzzleRoomManager NextRoom(int index)
    {
        if (index >= RoomManagers.Length - 1)
        {
            return FinalRoom;
        }
        currentRoomIndex = index + 1;
        return CurrentRoom;
    }

    public PuzzleRoomManager CurrentRoom
    {
        get { return RoomManagers[currentRoomIndex]; }
    }
}

