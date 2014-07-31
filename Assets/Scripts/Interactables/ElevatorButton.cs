using System;
using Assets.Scripts.Interactables;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

//TODO: Display shitty elevator instructions
public class ElevatorButton : ExaminableBase
{
    private RoomManager _manager;
    void Awake()
    {
        _manager = GetComponentInParent<RoomManager>();
    }

    public override IEnumerator Examine(Action callback)
    {
        InputManager.Instance.PlayerInputEnabled = false;
        yield return StartCoroutine(GetElevatorFloor((floor) =>
        {
            var room = _manager.GetRoom(floor);
            var entrance = room.GetEntrance();
            //room.OnRoomEnter();
            StartCoroutine(FindObjectOfType<InfoPlayer>().OnEnterDoorway(transform.forward, entrance, room));
            callback();
        }));
    }

    IEnumerator GetElevatorFloor(Action<int> callback)
    {
        selectingFloor = true;
        while (true)
        {

            if (selectedFloor != -1)
            {
                callback(selectedFloor);
                selectingFloor = false;
                selectedFloor = -1;
                yield break;
            }

            /*//:Effort:
            if (Input.GetKey(KeyCode.Alpha0))
            {
                callback(0);
                yield break;
            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                callback(1);
                yield break;
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                callback(2);
                yield break;
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                callback(3);
                yield break;
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                callback(4);
                yield break;
            }*/
            //Cancel doesn't immediately work, oh well
            /*if (Input.GetKey(KeyCode.C))
            {
                callback(-1);
                yield break;
            }*/
            yield return new WaitForEndOfFrame();
        }
    }



    private int selectedFloor = -1;
    private bool selectingFloor = false;
    void OnGUI()
    {
        if (selectingFloor)
        {
            GUI.Box(new Rect(Screen.width - (32 + 96), 64, 96, 256), "Select a floor");

            if (GUI.Button(new Rect(Screen.width - (32 + 96 - 16), 64 + 32, 64, 32), "F4")) selectedFloor = 4;

            if (GUI.Button(new Rect(Screen.width - (32 + 96 - 16), 64 + 64 + 8, 64, 32), "F3")) selectedFloor = 3;

            if (GUI.Button(new Rect(Screen.width - (32 + 96 - 16), 64 + 96 + 8 * 2, 64, 32), "F2")) selectedFloor = 2;

            if (GUI.Button(new Rect(Screen.width - (32 + 96 - 16), 64 + 128 + 8 * 3, 64, 32), "F1")) selectedFloor = 1;

            if (GUI.Button(new Rect(Screen.width - (32 + 96 - 16), 64 + 128 + 32 + 8 * 4, 64, 32), "Leave")) selectedFloor = 0;
        }
    }



}
