using UnityEngine;
using System.Collections;

//This should probably exist separate from the player and get spawned in by bootstrapped
public class CameraManager : Singleton<CameraManager>
{
    public Camera camera1;
    public Camera camera2;
    private const float rotateAmount = 180.0f;
    public Mesh shapeMesh; //Assigned in the inspector until I can figure out how to get a mesh programatically
    AnimationCurve curve;

    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(0f,0f,-500f);
        camera1 = Camera.main;
        camera2 = GetComponentInChildren<Camera>();
        StartCoroutine(DoWipeIn(1f));
    }


    public IEnumerator DoWipeIn(float time)
    {
        yield return StartCoroutine(ScreenWipe.use.ShapeWipe(camera2, camera1, time, ScreenWipe.ZoomType.Grow, shapeMesh, rotateAmount, curve));
    }

    public IEnumerator DoWipeOut(float time)
    {
        yield return StartCoroutine(ScreenWipe.use.ShapeWipe(camera1, camera2, time, ScreenWipe.ZoomType.Shrink, shapeMesh, rotateAmount, curve));
    }
}