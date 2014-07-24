using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class ExitDoorway : MonoBehaviour
{
    public int targetRoomIndex = -1;
    public bool StartsActive = false;
    private bool _isActive = false;
    private RoomManager _roomManager;

    public bool Active
    {
        get { return _isActive; }
    }

    void Start()
    {
        collider.isTrigger = false;
        if (renderer != null) renderer.material.color = Color.grey;

        _roomManager = GetComponentInParent<RoomManager>();
        if (_roomManager == null)
        {
            Debug.LogError("Expected exitdoorway to have a room manager parent");
        }

        if (StartsActive)
        {
            Activate();
        }
    }

    public void Activate(bool value = true)
    {
        _isActive = value;
        collider.isTrigger = value;
        if (renderer != null) renderer.material.color = value ? Color.cyan : Color.grey;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (_isActive && collider.GetComponent<InfoPlayer>() != null)
        {
            RoomManager nextRoom = targetRoomIndex > -1 
                ? _roomManager.GetRoom(targetRoomIndex) 
                : _roomManager.GetNextRoom();

            var entrance = nextRoom.GetEntrance();
            StartCoroutine(collider.GetComponent<InfoPlayer>().OnEnterDoorway(transform.forward, entrance));
            _roomManager.OnRoomExit();
            nextRoom.OnRoomEnter();
        }
    }
}
