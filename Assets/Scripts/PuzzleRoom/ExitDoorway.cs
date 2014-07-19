using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class ExitDoorway : MonoBehaviour
{
    private bool _isActive = false;

    public bool Active
    {
        get { return _isActive; }
    }

    public void Start()
    {
        collider.isTrigger = false;
        renderer.material.color = Color.grey;
    }

    public void Activate()
    {
        _isActive = true;
        collider.isTrigger = true;
        renderer.material.color = Color.cyan;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (_isActive && collider.GetComponent<InfoPlayer>() != null)
        {
            var nextRoom = transform.parent.GetComponentInParent<PuzzleRoomManager>().OnRoomExit();
            var entrance = nextRoom.GetEntrance();
            StartCoroutine(collider.GetComponent<InfoPlayer>().OnEnterDoorway(transform.forward, entrance));
        }
    }
}
