using UnityEngine;

public class RoomManager : MonoBehaviourBase
{
    private EntranceDoorway _entrance;
    private RoomWorkflow _workflow;
    public int RoomIndex;
    protected void Awake()
    {
        _entrance = GetComponentInChildren<EntranceDoorway>();
        _workflow = GetComponentInParent<RoomWorkflow>();
    }

    public Transform GetEntrance()
    {
        return _entrance.transform;
    }

    public RoomManager GetRoom(int index)
    {
        //TODO This is really dumb
        return _workflow.NextRoom(index - 1);
    }

    public RoomManager GetNextRoom()
    {
        return _workflow.NextRoom(RoomIndex);
    }


    public virtual void OnRoomEnter()
    {

    }

    public virtual void OnRoomExit()
    {

    }

}