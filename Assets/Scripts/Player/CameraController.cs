using Assets.Scripts.GameState;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Collections;

//This should probably exist separate from the player and get spawned in by bootstrapped
public class CameraController : Singleton<CameraController>
{

    private Transform cameraRig;

    // Use this for initialization
    void Start()
    {
        //This should be ok for now since there aren't multiple cameras flying around
        cameraRig = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //Move the camera on the corresponding input axis
        //TODO: Rotate the camera based on the direction the player is facing
        //TODO: Center the camera behind the player if the press the center camera button
        //TODO: Remove manual camera axis stuff here

        /*if (InputManager.Instance.CameraAction)
        {
            iTween.RotateTo(cameraRig.gameObject, cameraRig.parent.FindChild("Character").rotation.eulerAngles, 1f);
        }*/

        //cameraRig.Rotate(Vector3.up, Input.GetAxis("Camera Horizontal") * invertCameraHorz, Space.World);
        //cameraRig.Rotate(Vector3.right, Input.GetAxis("Camera Vertical") * invertCameraVert, Space.Self);

        //Limit camera movement
        /*if (cameraRig.rotation.eulerAngles.x < 5f)
        {
            cameraRig.rotation = Quaternion.Euler(5f, cameraRig.rotation.eulerAngles.y, 0f);
        }
        else if (cameraRig.rotation.eulerAngles.x > 75f)
        {
            cameraRig.rotation = Quaternion.Euler(75f, cameraRig.rotation.eulerAngles.y, 0f);
        }*/
    }
}
