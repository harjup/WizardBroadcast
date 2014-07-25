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
            var entrance = _manager.GetRoom(floor).GetEntrance();
            StartCoroutine(FindObjectOfType<InfoPlayer>().OnEnterDoorway(transform.forward, entrance));
            callback();
        }));
    }

    IEnumerator GetElevatorFloor(Action<int> callback)
    {
        while (true)
        {
            //:Effort:
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
            }
            //Cancel doesn't immediately work, oh well
            /*if (Input.GetKey(KeyCode.C))
            {
                callback(-1);
                yield break;
            }*/
            yield return new WaitForEndOfFrame();
        }
    }






}
