using System;
using System.Collections;
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

        private bool waitingForCallback = false;
        private bool _blockEngaged = false;

        void Start()
        {
            
        }

        void Update()
        {
            if (waitingForCallback) return;

            if (_examinableObject != null)
            {
                if (InputManager.Instance.InteractAction)
                {
                    ExamineObject();
                }   
            }
            if (_pushableObject != null)
            {
                if (InputManager.Instance.ClimbButton)
                {
                    waitingForCallback = true;
                    StartCoroutine(GetComponent<UserMovement>().ClimbBlock(_pushableObject, () =>
                    {
                        waitingForCallback = false;
                    }));
                    return;
                }


                if (!InputManager.Instance.InteractAction) return;
                if (!_blockEngaged)
                {
                    waitingForCallback = true;
                    StartCoroutine(GetComponent<UserMovement>().EngageBlock(_pushableObject, () =>
                    {
                        waitingForCallback = false;
                        _blockEngaged = true;
                    }));
                    return;
                }

                waitingForCallback = true;
                StartCoroutine(GetComponent<UserMovement>().DisengageBlock(() =>
                {
                    waitingForCallback = false;
                    _blockEngaged = false;
                }));



            }
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
                StartCoroutine(GetComponent<PlayerAnimate>().GetMessedUp());
            }
        }

        void OnTriggerExit(Collider other)
        {
            var component = other.GetComponent<MonoBehaviour>();

            if (component as PushableBase != null && !GetComponent<UserMovement>().GetBlockEngaged()) //!_blockEngaged)
            {
                _pushableObject = null;
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
