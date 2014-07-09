using UnityEngine;
using System.Collections;

namespace WizardBroadcast
{
    using System;

    public class MovementPlayer : MonoBehaviourBase
    {
        //This is dirty and I hate it
        //TODO: Put this logic in an input controller or some shit
        public bool disableMovement = false;

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

            playerMesh = transform.FindChild("Character");
            rigidBody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (disableMovement)
                return;

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
        }
    }
}