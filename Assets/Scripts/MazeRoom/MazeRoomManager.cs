using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class MazeRoomManager : RoomManager
{

    private MazeCamera _roomCamera;

    private static Color[] _lightColors =
    {
        Color.cyan,
        Color.yellow, 
        new Color(157f,28f,237f), //purple
        Color.green, 
        Color.red, 
        new Color(237f,147f,28f), //orange
        Color.blue,
        Color.magenta,
        Color.grey, 
    };

    new void Awake()
    {
        base.Awake();
        _roomCamera = GetComponentInChildren<MazeCamera>();
    }
    public override void OnRoomEnter()
    {
        FindObjectOfType<Light>().color = _lightColors[RoomIndex];

        _roomCamera.Enabled = true;
    }
    public override void OnRoomExit()
    {
        _roomCamera.Enabled = false;
    }
}
