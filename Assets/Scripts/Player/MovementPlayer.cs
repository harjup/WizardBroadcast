using UnityEngine;
using System.Collections;

namespace WizardBroadcast
{
    using System;

    public class MovementPlayer : MonoBehaviourBase
    {
        public const float MaxSpeed = 20;

        private Vector3 walkVector;
        private Rigidbody rigidBody;

        private Transform playerMesh;
        private Transform cameraRig;
        private BoxCollider interactTrigger;
        private int invertCameraHorz = -1;
        private int invertCameraVert = 1;

        // Use this for initialization
        private void Start()
        {
            playerMesh = transform.FindChild("Player Mesh");
            interactTrigger = playerMesh.GetComponent<BoxCollider>();
            cameraRig = transform.FindChild("Camera Rig");
            rigidBody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            #region General Movement
            //Create the reference axis based on the camera rotation, ignoring y rotation
            var forward = cameraRig.TransformDirection(Vector3.forward).SetY(0).normalized;
            var right = new Vector3(forward.z, 0.0f, -forward.x);

            //Set the player's walk direction
            walkVector = (Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward);
            
            //prevent the player from moving faster when walking diagonally
            if (walkVector.sqrMagnitude > 1f)
               walkVector = walkVector.normalized;
            
            //Rotate the player to face direction of movement only when input keys are pressed
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
               playerMesh.rotation = Quaternion.LookRotation(this.walkVector.SetY(0), Vector3.up);

            //Set direction and speed
            rigidBody.velocity = walkVector * MaxSpeed;

            #endregion

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
}