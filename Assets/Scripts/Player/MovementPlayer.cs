using Assets.Scripts.Managers;
using Assets.Scripts.Repository;
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
        private Transform cameraRig;


        private Transform playerMesh;


        // Use this for initialization
        private void Start()
        {
            //This should be ok for now since there aren't multiple cameras flying around
            cameraRig = Camera.main.transform.parent;
            //Let's have it be free from the palyer or sommin
            cameraRig.parent = null;

            playerMesh = transform.FindChild("Character");
            rigidBody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            MovePlayer();
           
        }

        private void FixedUpdate()
        {
            MoveCamera();
        }

        void MovePlayer()
        {
            //Create the reference axis based on the camera rotation, ignoring y rotation
            var forward = cameraRig.TransformDirection(Vector3.forward).SetY(0).normalized;
            var right = new Vector3(forward.z, 0.0f, -forward.x);

            //Set the player's walk direction
            walkVector = (
                InputManager.Instance.HoritzontalAxis * right
                + InputManager.Instance.VerticalAxis * forward
                );

            //prevent the player from moving faster when walking diagonally
            if (walkVector.sqrMagnitude > 1f)
                walkVector = walkVector.normalized;

            //Rotate the player to face direction of movement only when input keys are pressed
            //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            if (Math.Abs(InputManager.Instance.RawHoritzontalAxis) >= 1
                || Math.Abs(InputManager.Instance.RawVerticalAxis) >= 1)
                playerMesh.rotation = Quaternion.LookRotation(this.walkVector.SetY(0), Vector3.up);

            //Set direction and speed
            rigidBody.velocity = walkVector * MaxSpeed;
        }

        void MoveCamera()
        {
            //TODO: Rotate the camera based on the direction the player is facing
            //TODO: Center the camera behind the player if the press the center camera button
            //TODO: Remove manual camera axis stuff here


            var positionDifference = playerMesh.position - cameraRig.position;
            float xSpeed  = Mathf.Abs(positionDifference.x) * 5f;
            float zSpeed = Mathf.Abs(positionDifference.z) * 5f;
            if (Mathf.Abs(positionDifference.x) >= .5f)
            {
                cameraRig.position = cameraRig.position.SetX(iTween.FloatUpdate(cameraRig.position.x, playerMesh.position.x, xSpeed));
            }
            if (Mathf.Abs(positionDifference.z) >= .5f)
            {
                cameraRig.position = cameraRig.position.SetZ(iTween.FloatUpdate(cameraRig.position.z, playerMesh.position.z, zSpeed));
            }

            var rotationDifference = Mathf.Abs(cameraRig.gameObject.transform.eulerAngles.y
                                          - playerMesh.rotation.eulerAngles.y);

            if (InputManager.Instance.CameraAction)
            {
                iTween.Stop(cameraRig.gameObject);
                iTween.RotateTo(cameraRig.gameObject,
                    cameraRig.rotation.eulerAngles.SetY(playerMesh.rotation.eulerAngles.y),
                    1f);
            }
            /*else if ((rotationDifference < 150 || rotationDifference > 220)
                && (cameraRig.gameObject.GetComponent<iTween>() == null 
                    || !cameraRig.gameObject.GetComponent<iTween>().isRunning))
            {

                var rotationMagnitude = 400f / (Mathf.Abs(rotationDifference - 180) + 40);
                iTween.RotateUpdate(cameraRig.gameObject,
                    cameraRig.rotation.eulerAngles.SetY(playerMesh.rotation.eulerAngles.y), rotationMagnitude);
            }*/
        }

        void OnGUI()
        {
            var rotaitonDifference =
                Mathf.Abs(cameraRig.gameObject.transform.eulerAngles.y - playerMesh.rotation.eulerAngles.y);
            var rotationMagnitude = 40f / (Mathf.Abs(rotaitonDifference - 180) + 40);

            GUI.TextField(new Rect(10, 100, 100, 25), rotaitonDifference.ToString());
            
            
            GUI.TextField(new Rect(10, 125, 100, 25), rotationMagnitude.ToString());
            
        }

    }
}