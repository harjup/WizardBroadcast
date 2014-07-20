using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using Assets.Scripts.Repository;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using System.Collections;

namespace WizardBroadcast
{
    using System;

    public class UserMovement : MonoBehaviourBase
    {
        public const float MaxSpeed = 15;

        private Vector3 walkVector;
        private Rigidbody rigidBody;
        public Transform cameraRig;

        private Transform playerMesh;

        private bool _blockEngaged = false;
        private Vector3 pushDirection;

        private PushableBase _pushPoint;

        private bool inAir = false;


        // Use this for initialization
        private void Start()
        {
            //This should be ok for now since there aren't multiple cameras flying around
            //TODO: There are multiple cameras flying around make sure this is alright
            var userCamera = GetComponentInChildren<Camera>();
            CameraManager.Instance.SetMainCamera(userCamera);
            cameraRig = userCamera.transform.parent;

            //Let's have it be free from the player or sommin
            cameraRig.parent = null;
            DontDestroyOnLoad(cameraRig);
            
            playerMesh = transform.FindChild("Character");
            rigidBody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            //If in a pushing state do a pushthing
            //If The logic is similar enough in pushing and walking we can combine them
            //Or block pusing can be in its own component I dunno
            if (_blockEngaged)
            {
                PushMovement();
                return;
            }

            //If in walking state then do a moveplayer
            MovePlayer();
        }

        private void FixedUpdate()
        {
            MoveCamera();
        }

        public void RotateTo(Vector3 rotation)
        {
            playerMesh.rotation = Quaternion.Euler(playerMesh.eulerAngles.SetY(rotation.y));
        }
        public void LookAt(Vector3 vector3)
        {
            playerMesh.LookAt(vector3.SetY(playerMesh.position.y));
        }
        public Vector3 Forward
        {
            get { return playerMesh.forward; }
        }


        void MovePlayer()
        {
            var forward = Vector3.forward;
            //Create the reference axis based on the camera rotation, ignoring y rotation
            //We're getting the main camera, which should be the one that's enabled. It's null if disabled so don't do anything if so
            if (Camera.main != null)
            {    
                forward = Camera.main.transform.TransformDirection(Vector3.forward).SetY(0).normalized;
            }
            
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

            var velocity = walkVector * MaxSpeed;

            //Set direction and speed
            rigidBody.velocity = rigidBody.velocity
                                        .SetX(velocity.x)
                                        .SetY(inAir ? -20f: 0f)
                                        .SetZ(velocity.z);
        }

        public bool AirState
        {
          get { return inAir; }
          set {inAir = value;}
        }

        private bool pushing = false;
        void PushMovement()
        {
            if (pushing) return;
            if (Math.Abs(InputManager.Instance.RawVerticalAxis) < .001) return;

            pushing = true;
            StartCoroutine(_pushPoint.GetPushBlock().PushAction(pushDirection,
                (disengage) =>
                {
                    pushing = false;
                    if (disengage) { StartCoroutine(DisengageBlock(() => { })); }                        
                }));
        }

        //TODO: Put in the timing things like walking to the block I dunno
        public IEnumerator EngageBlock(PushableBase block, Action doneAction)
        {
            _pushPoint = block;
            //Get push direction for block (+/-X or +/-Z)
            //pushDirection = block.transform.forward;
            //Orient player toward block
            GetComponent<UserMovement>().RotateTo(block.GetOrientation());
            //Move player next to block
            transform.position = block.GetPosition().SetY(transform.position.y);

            pushDirection = GetComponent<UserMovement>().playerMesh.forward;

            //child block to player
            if (!block.GetPushBlock().isSlippery)
            {
                transform.parent = block.transform.parent;
            }
            
            //Set input type to block pushing
            _blockEngaged = true;

            doneAction();
            yield return null;
        }

        //TODO: Put in the timing things like waiting for an animation I dunno
        public IEnumerator DisengageBlock(Action doneAction)
        {
            //unchild block from player
            //block.GetParent().parent = null;
            transform.parent = null;
            transform.position -= pushDirection * 1f;
            //Enable walking movement
            _blockEngaged = false;
            doneAction();

            yield return null;
        }

        public bool GetBlockEngaged()
        {
            return _blockEngaged;
        }

        void MoveCamera()
        {
            cameraRig.position = cameraRig.position.SetY(iTween.FloatUpdate(cameraRig.position.y, playerMesh.position.y, 5f));

            //TODO: Have the camera contextually recenter or some shit
            var positionDifference = playerMesh.position - cameraRig.position;
            float xSpeed  = Mathf.Abs(positionDifference.x) * 5f;
            float zSpeed = Mathf.Abs(positionDifference.z) * 5f;

            //Cap the camera's speed so it doesn't go fucking nuts and start overshooting the player
            if (xSpeed > 40 ) { xSpeed = 40; }
            if (zSpeed > 40 ) { zSpeed = 40; }

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

        public IEnumerator ClimbBlock(PushableBase pushableObject, Action action)
        {
            if (pushableObject.GetPushBlock().TopBlocked())
            {
                action();
                yield break;
            }

            iTween.MoveTo(gameObject,
                pushableObject.GetPushBlock().transform.position.SetY(pushableObject.GetPushBlock().transform.position.y + 2.5f), 
                .4f);
            action();

            yield break;
        }

        public IEnumerator ClimbGeometry(Vector3 target, Action action)
        {
            collider.enabled = false;

            iTween.MoveTo(gameObject,
                target.SetY(target.y + 2.5f),
                .4f);
            action();
            yield return new WaitForSeconds(.4f);
            collider.enabled = true;
            yield break;
        }


        
    }
}
