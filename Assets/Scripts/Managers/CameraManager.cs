using Assets.Scripts.GameState;
using UnityEngine;
using System.Collections;

//This should probably exist separate from the player and get spawned in by bootstrapped
public class CameraManager : Singleton<CameraManager>
{
    private Camera _mainCamera;
    private Camera _playerCamera;
    private Camera _transitionCamera;
    private const float RotateAmount = 180.0f;
    public Mesh shapeMesh; //Assigned in the inspector until I can figure out how to get a mesh programatically
    AnimationCurve curve; //No idea what this does or if it's nessesary woo

    // Use this for initialization
    void Awake()
    {
        transform.position = new Vector3(0f,0f,-500f);
        _mainCamera = _playerCamera = GetComponentInChildren<CameraController>().camera;
        if (_playerCamera == null)
        {
            Debug.LogError("Camera maanger couldn't find player camera");
        }
        _transitionCamera = transform.FindChild("Camera2").GetComponent<Camera>();
        if (_transitionCamera == null)
        {
            Debug.LogError("Camera maanger couldn't find transition camera");
        }
    }

    void Start()
    {
        StartCoroutine(DoWipeIn(1f));
        Init();
    }

    //TODO: Put this logic in the level itself, the camera manager shouldn't care
    private void Init()
    {
        if (SceneMap.GetSceneFromStringName(Application.loadedLevelName) == Scene.Level3
            || SceneMap.GetSceneFromStringName(Application.loadedLevelName) == Scene.Start)
        {
            _playerCamera.enabled = false;
        }
        else
        {
            _playerCamera.enabled = true;
        }
    }

    
    void OnLevelWasLoaded(int level)
    {
        Init();
    }

    public IEnumerator DoWipeIn(float time)
    {
        yield return StartCoroutine(ScreenWipe.use.ShapeWipe(_transitionCamera, _mainCamera, time, ScreenWipe.ZoomType.Grow, shapeMesh, RotateAmount, curve));
    }

    public IEnumerator DoWipeOut(float time)
    {
        yield return StartCoroutine(ScreenWipe.use.ShapeWipe(_mainCamera, _transitionCamera, time, ScreenWipe.ZoomType.Shrink, shapeMesh, RotateAmount, curve));
    }

    public Transform GetCameraRig()
    {
        return _playerCamera.transform.parent;
    }

    public Camera GetPlayerCamera()
    {
        return _playerCamera;
    }
}