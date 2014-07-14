using UnityEngine;
using System.Collections;

//This should probably exist separate from the player and get spawned in by bootstrapped
public class CameraManager : Singleton<CameraManager>
{
    private Camera _mainCamera;
    private Camera _transitionCamera;
    private const float RotateAmount = 180.0f;
    public Mesh shapeMesh; //Assigned in the inspector until I can figure out how to get a mesh programatically
    AnimationCurve curve; //No idea what this does or if it's nessesary woo

    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(0f,0f,-500f);
        _mainCamera = Camera.main;
        _transitionCamera = GetComponentInChildren<Camera>();
        StartCoroutine(DoWipeIn(1f));
    }

    public IEnumerator DoWipeIn(float time)
    {
        yield return StartCoroutine(ScreenWipe.use.ShapeWipe(_transitionCamera, _mainCamera, time, ScreenWipe.ZoomType.Grow, shapeMesh, RotateAmount, curve));
    }

    public IEnumerator DoWipeOut(float time)
    {
        yield return StartCoroutine(ScreenWipe.use.ShapeWipe(_mainCamera, _transitionCamera, time, ScreenWipe.ZoomType.Shrink, shapeMesh, RotateAmount, curve));
    }

    public void SetMainCamera(Camera camera)
    {
        _mainCamera = camera;
    }

    public Transform GetCameraRig()
    {
        return _mainCamera.transform.parent;
    }
}