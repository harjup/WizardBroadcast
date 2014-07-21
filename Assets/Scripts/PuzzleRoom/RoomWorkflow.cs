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
    public RoomManager FirstRoomAlt;

    public bool ReverseRooms = false;

    [SerializeField]
    private int currentRoomIndex = 0;

    void Awake()
    {
        InitRooms();
    }


    void InitRooms(bool isReverseOrder = false)
    {
        RoomManagers = GetComponentsInChildren<RoomManager>();
        currentRoomIndex = 0;

        //I don't want the final room in the room managers list it screws stuff up when going backwards
        var managers = new List<RoomManager>();
        for (int i = 0; i < RoomManagers.Length; i++)
        {
            if (RoomManagers[i] == FinalRoom || RoomManagers[i] == FirstRoomAlt)
            {
                RoomManagers[i].RoomIndex = -1;
                continue;
            }
            RoomManagers[i].RoomIndex = i;
            managers.Add(RoomManagers[i]);
        }
        RoomManagers = managers.ToArray();
    }


    public RoomManager FirstRoom()
    {
        currentRoomIndex = 0;
        return CurrentRoom;
    }

    public RoomManager PreviousRoom(int index)
    {
        if (index <= -1)
        {
            return FinalRoom;
        }
        currentRoomIndex = index - 1;
        return CurrentRoom;
    }

    public RoomManager GetRoom(int index)
    {
        currentRoomIndex = index;
        return CurrentRoom;
    }

    public RoomManager NextRoom(int index)
    {
        currentRoomIndex = index + 1;

        if (ReverseRooms)
        {
            currentRoomIndex = index - 1;

            if (currentRoomIndex <= 0)
            {
                return FirstRoomAlt;
            }
        }

        if (currentRoomIndex >= RoomManagers.Length)
        {
            return FinalRoom;
        }

        return CurrentRoom;
    }

    public RoomManager CurrentRoom
    {
        get { return RoomManagers[currentRoomIndex]; }
    }
}

