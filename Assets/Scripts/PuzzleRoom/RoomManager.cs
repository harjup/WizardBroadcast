using UnityEngine;

public class RoomManager : MonoBehaviourBase
{
    private EntranceDoorway _entrance;

    void Awake()
    {
        _entrance = GetComponentInChildren<EntranceDoorway>();
    }

    public Transform GetEntrance()
    {
        return _entrance.transform;
    }
}