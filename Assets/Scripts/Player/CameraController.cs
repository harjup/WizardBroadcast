using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    private Transform cameraRig;
    private int invertCameraHorz = -1;
    private int invertCameraVert = 1;
    

    // Use this for initialization
    void Start()
    {
        //This should be ok for now since there aren't multiple cameras flying around
        cameraRig = Camera.main.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

        #region Camera Movement
        //Move the camera on the corresponding input axis
        cameraRig.Rotate(Vector3.up, Input.GetAxis("Camera Horizontal") * invertCameraHorz, Space.World);
        cameraRig.Rotate(Vector3.right, Input.GetAxis("Camera Vertical") * invertCameraVert, Space.Self);

        //Limit camera movement
        if (cameraRig.rotation.eulerAngles.x < 5f)
        {
            cameraRig.rotation = Quaternion.Euler(5f, cameraRig.rotation.eulerAngles.y, 0f);
        }
        else if (cameraRig.rotation.eulerAngles.x > 75f)
        {
            cameraRig.rotation = Quaternion.Euler(75f, cameraRig.rotation.eulerAngles.y, 0f);
        }

        #endregion
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, Screen.height - 60, 100, 50), "Invert Camera"))
        {
            invertCameraHorz *= -1;
            invertCameraVert *= -1;
        }
    }
}
