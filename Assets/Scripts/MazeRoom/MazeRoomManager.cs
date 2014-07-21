using System.Runtime.InteropServices;
using Assets.Scripts.Interactables;
using UnityEngine;
using System.Collections;

public class MazeRoomManager : RoomManager
{

    private MazeCamera _roomCamera;

    private MazeExitSet _exitSet;

    private static Color[] _lightColors =
    {
        Color.cyan,
        Color.yellow, 
        new Color(.61f,.11f,.92f, 1f), //purple
        Color.green, 
        Color.red, 
        new Color(.9f,.57f,.1f, 1f), //orange
        Color.blue,
        Color.magenta,
        Color.grey
    };

    new void Awake()
    {
        base.Awake();
        _roomCamera = GetComponentInChildren<MazeCamera>();
        _exitSet = GetComponentInChildren<MazeExitSet>();
    }
    public override void OnRoomEnter()
    {
        FindObjectOfType<Light>().color = RoomIndex <= -1 
            ? Color.grey 
            : _lightColors[RoomIndex];
        
        if (_workflow.FinalRoom == this)
        {
            _workflow.ReverseRooms = true;    
        }
        if (RoomIndex == 0 && _workflow.ReverseRooms)
        {
            //Spawn the wizard and helping hand, disable all exits
            _exitSet.DisableExits();
        }

        _exitSet.InitExitSet();
        _roomCamera.Enabled = true;
    }
    public override void OnRoomExit()
    {
        _roomCamera.Enabled = false;
    }
}
