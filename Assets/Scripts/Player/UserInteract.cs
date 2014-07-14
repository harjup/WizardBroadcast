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
                if (InputManager.Instance.InteractAction)
                {
                    PushObject();
                }   
            }
        }

        private void PushObject()
        {
            //TODO: Just tell userMovement to engage/disengage the block based on state for now I suppose
            //most of the stuff we need to know is in usermovement so might as well start in there
            
            waitingForCallback = true;
            //Center camera
            //Switch movement state to pushing or some shit

            //Look at the block
            GetComponent<UserMovement>().RotateTo(_pushableObject.GetOrientation());

            //Move up to the block's edge so you're touching or whatever.
            //TODO: Only set position on axis we're pushing in.
            transform.position = _pushableObject.GetPosition().SetY(transform.position.y);
            _pushableObject.GetParent().parent = transform;

            //Set input type to block pushing
            //Either do this by triggering state on userMovement
            //Or just tell the input manager to transmit different input signals or some shit

            waitingForCallback = false;
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
            //If we are entering an examinable set the current examinable to it
            if (component as ExaminableBase != null)
            {
                _examinableObject = component as ExaminableBase;
            }

            if (component as PushableBase != null)
            {
                _pushableObject = component as PushableBase;
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

            if (component as PushableBase != null)
            {
                _pushableObject = null;
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
