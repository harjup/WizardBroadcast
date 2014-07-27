using System.Runtime.InteropServices;
using Assets.Scripts.Interactables;
using UnityEngine;
using System.Collections;

public class MazeRoomManager : RoomManager
{
    private const float HelpingHandSpawnTime = 30f;
    private static GameObject _helpingHandPrefab;
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

        if (_helpingHandPrefab == null)
        {
            _helpingHandPrefab = Resources.Load("Prefabs/HelpingHand") as GameObject;
        }

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
        //Nevermind do nothing ignore this
        /*if (RoomIndex == -1 && _workflow.ReverseRooms)
        {
            
            //_exitSet.DisableExits();
        }*/
        if (_exitSet != null) _exitSet.InitExitSet();
        if (_roomCamera != null) _roomCamera.Enabled = true;

        if (RoomIndex == 6)
        {
            CameraManager.Instance.GetPlayerCamera().enabled = true;
        }

        //Lame, StopCoroutine only works with strings
        StopCoroutine("SpawnHelpingHand");
        if (RoomIndex > 0 && RoomIndex < 7 //Spawn a helping hand on indexes 1-6
            || RoomIndex == 7 && !_workflow.ReverseRooms) //If the index is 7 only spawn one if the rooms are not reversed
        {
            StartCoroutine("SpawnHelpingHand");
        } 
    }
    public override void OnRoomExit()
    {
        if (_roomCamera != null) _roomCamera.Enabled = false;

        if (RoomIndex == 6)
        {
            CameraManager.Instance.GetPlayerCamera().enabled = false;
        }

        StopCoroutine("SpawnHelpingHand");
    }

    IEnumerator SpawnHelpingHand()
    {
        yield return new WaitForSeconds(HelpingHandSpawnTime);
        //Say something stupid
        Instantiate(_helpingHandPrefab, Vector3.zero, Quaternion.identity);
    }
}
