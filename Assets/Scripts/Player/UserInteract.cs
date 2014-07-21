using System;
using System.Collections;
using Assets.Scripts.GameState;
using Assets.Scripts.Interactables;
using Assets.Scripts.Managers;
using UnityEngine;
using WizardBroadcast;

namespace Assets.Scripts.Player
{
    public class UserInteract : MonoBehaviourBase
    {
        private ExaminableBase _examinableObject;
        private PushableBase _pushableObject;


        private float playerHeight = 2.5f;
        private Vector3 _climbTarget;

        private bool waitingForCallback = false;
        private bool _blockEngaged = false;

        private UserMovement _userMovement;
        void Awake()
        {
            _userMovement = GetComponent<UserMovement>();
        }

        void Update()
        {
            ExamineInput();
            PushBlockInput();
            ClimbInput();

            if (_userMovement.GetBlockEngaged()) return;
            CheckForClimbableSurfaces();
        }

        void ExamineInput()
        {
            if (waitingForCallback) return;
            if (_examinableObject == null) return;

            if (InputManager.Instance.InteractAction)
            {
                ExamineObject();
            }
        }

        void PushBlockInput()
        {
            if (waitingForCallback) return;
            if (_pushableObject == null) return;
            if (!InputManager.Instance.InteractAction) return;
            if (!_blockEngaged)
            {
                waitingForCallback = true;
                StartCoroutine(_userMovement.EngageBlock(_pushableObject, () =>
                {
                    waitingForCallback = false;
                    _blockEngaged = true;
                }));
                return;
            }

            waitingForCallback = true;
            StartCoroutine(_userMovement.DisengageBlock(() =>
            {
                waitingForCallback = false;
                _blockEngaged = false;
            }));
        }
        void ClimbInput()
        {
            if (waitingForCallback) return;
            if (!InputManager.Instance.ClimbButton) return; //Don't climb if we're not pushing the button
            if (_climbTarget == Vector3.zero) return;       //Don't climb if we don't get a valid target
            if (_userMovement.GetBlockEngaged()) return;    //Don't climb if you're pushing a block
            if (_userMovement.AirState) return;             //Don't climb if you're in the air
            

            waitingForCallback = true;
            StartCoroutine(_userMovement.ClimbGeometry(_climbTarget, () =>
            {
                waitingForCallback = false;
            }));
        }


        void ExamineObject()
        {
            waitingForCallback = true;
            InputManager.Instance.PlayerMovementEnabled = false;
            InputManager.Instance.CameraControlEnabled = false;

            //TODO: Determine a decent method of accessing usermovement for reorienting the player mesh or whatever
            GetComponent<UserMovement>().LookAt(_examinableObject.transform.position);

            Debug.Log(_examinableObject.gameObject.transform.position.SetY(transform.position.y));
            StartCoroutine(_examinableObject.Examine(() =>
            {
                waitingForCallback = false;
                InputManager.Instance.PlayerMovementEnabled = true;
                InputManager.Instance.CameraControlEnabled = true;
            }));
        }

        void OnTriggerEnter(Collider other)
        {
            var component = other.GetComponent<MonoBehaviour>();

            if (component as PushableBase != null 
                && (component as PushableBase).IsEnabled())
            {
                _pushableObject = component as PushableBase;
            }

            if (component as DeathVolume != null)
            {

                if (SceneMap.GetSceneFromStringName(Application.loadedLevelName) == Scene.Level2)
                {
                    waitingForCallback = true;
                    StartCoroutine(_userMovement.DisengageBlock(() =>
                    {
                        waitingForCallback = false;
                        _blockEngaged = false;
                        _pushableObject = null;
                        StartCoroutine(GetComponent<PlayerAnimate>().GetMessedUp());
                    }));
                }
                //Pretty really bad right here
                //I want to go back to the hub is a hand gets the player at the last room in lvl3
                else if (SceneMap.GetSceneFromStringName(Application.loadedLevelName) == Scene.Level3 
                    && FindObjectOfType<RoomWorkflow>().CurrentRoom.RoomIndex == 0
                    && FindObjectOfType<RoomWorkflow>().ReverseRooms)
                {
                    Application.LoadLevel(SceneMap.GetScene(Scene.Hub));
                }
                else
                {
                    StartCoroutine(GetComponent<PlayerAnimate>().GetMessedUp());
                }

            }
        }

        void OnTriggerExit(Collider other)
        {
            var component = other.GetComponent<MonoBehaviour>();

            if (component as PushableBase != null)
            {
                if (!_userMovement.GetBlockEngaged())
                {
                    _pushableObject = null;
                }
                else if (_userMovement.AirState || _pushableObject.GetPushBlock().isSlippery)
                {
                    StartCoroutine(_userMovement.DisengageBlock(() =>
                    {
                        waitingForCallback = false;
                        _blockEngaged = false;
                        _pushableObject = null;
                    }));
                }
            }
        }

        public void OnInteractionTriggerEnter(Collider other)
        {
            var component = other.GetComponent<MonoBehaviour>();
            //If we are entering an examinable set the current examinable to it
            if (component as ExaminableBase != null)
            {
                _examinableObject = component as ExaminableBase;
            }
        }

        public void OnInteractionTriggerExit(Collider other)
        {
            var component = other.GetComponent<MonoBehaviour>();
            //If we are exiting an examinable set the current examinable to null
            if (component as ExaminableBase != null)
            {
                _examinableObject = null;
            }
        }

        private void CheckForClimbableSurfaces()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, _userMovement.Forward, out hit))
            {
                if (hit.distance < 2.5f)
                {
                    float maxHeight = 0f;
                    var playerTop = transform.position.y + playerHeight / 2f;
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(hit.transform.position.SetY(hit.transform.position.y + 10f), Vector3.down, 100);
                    foreach (var raycastHit in hits)
                    {
                        var distance = Mathf.Abs(raycastHit.point.y - playerTop);
                        if (!(distance < .5f)) continue;
                        _climbTarget = raycastHit.point;
                        return;
                    }
                }
            }
            _climbTarget = Vector3.zero;
        }

        void OnGUI()
        {
            if ((_examinableObject != null || _pushableObject != null) 
                && !waitingForCallback)
            {
                GuiManager.Instance.DrawInteractionPrompt();
            }
        }

    }
}
