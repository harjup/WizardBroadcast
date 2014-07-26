using UnityEngine;

public class RoomManager : MonoBehaviourBase
{
    protected EntranceDoorway _entrance;
    protected RoomWorkflow _workflow;
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
        return _workflow.GetRoom(index);
    }

    public RoomManager GetNextRoom()
    {
        return _workflow.NextRoom(RoomIndex);
    }


    public virtual void OnRoomEnter()
    {
        MusicManager.Instance.TransitionSongs(RoomIndex >= _workflow.SongIndex ? 1 : 0);
    }

    public virtual void OnRoomExit()
    {

    }

}