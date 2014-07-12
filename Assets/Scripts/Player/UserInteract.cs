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

        private bool waitingForCallback = false;


        void Start()
        {
            
        }

        void Update()
        {
            if (_examinableObject != null && !waitingForCallback)
            {
                if (InputManager.Instance.InteractAction)
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
                
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var component = other.GetComponent<MonoBehaviour>();
            //If we are entering an examinable set the current examinable to it
            if (component as ExaminableBase != null)
            {
                _examinableObject = component as ExaminableBase;
            }
        }

        void OnTriggerExit(Collider other)
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
            if (_examinableObject != null && !waitingForCallback)
            {
                GuiManager.Instance.DrawInteractionPrompt();
            }
        }

    }
}
