using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Utilities;
using UnityEngine;

public class RoomWorkflow : MonoBehaviourBase
{
    public RoomManager[] RoomManagers;
    public RoomManager FinalRoom;

    [SerializeField]
    private int currentRoomIndex = 0;

    void Awake()
    {
        RoomManagers = GetComponentsInChildren<RoomManager>();
        currentRoomIndex = 0;

        for (int i = 0; i < RoomManagers.Length; i++)
        {
            RoomManagers[i].RoomIndex = i;
        }
    }

    public RoomManager FirstRoom()
    {
        currentRoomIndex = 0;
        return CurrentRoom;
    }

    public RoomManager NextRoom(int index)
    {
        if (index >= RoomManagers.Length - 1)
        {
            return FinalRoom;
        }
        currentRoomIndex = index + 1;
        return CurrentRoom;
    }

    public RoomManager CurrentRoom
    {
        get { return RoomManagers[currentRoomIndex]; }
    }
}

