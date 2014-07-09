﻿using System;
using System.Collections;
using Assets.Scripts.Interactables;
using UnityEngine;
using WizardBroadcast;

namespace Assets.Scripts.Player
{
    public class InteractPlayer : MonoBehaviourBase
    {
        private IExaminable _examinableObject;
        private Texture _dismissalPromptGraphic;

        private bool waitingForCallback = false;


        void Start()
        {
            _dismissalPromptGraphic = Resources.Load<Texture>("Textures/dismissPrompt");
        }

        void Update()
        {
            if (_examinableObject != null && !waitingForCallback)
            {
                //TODO: Input manager so it knows we're holding the key down
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    waitingForCallback = true;

                    //Gross Gross Gross
                    //TODO: NO, BAD
                    GetComponent<MovementPlayer>().disableMovement = true;
                    StartCoroutine(_examinableObject.Examine(() =>
                    {
                        waitingForCallback = false; 
                        GetComponent<MovementPlayer>().disableMovement = false;
                    }));
                }
                
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var component = other.GetComponent<MonoBehaviour>();
            //If we are entering an examinable set the current examinable to it
            if (component as IExaminable != null)
            {
                _examinableObject = component as IExaminable;
            }
        }

        void OnTriggerExit(Collider other)
        {
            var component = other.GetComponent<MonoBehaviour>();
            //If we are exiting an examinable set the current examinable to null
            if (component as IExaminable != null)
            {
                _examinableObject = null;
            }
        }

        void OnGUI()
        {
            if (_examinableObject != null && !waitingForCallback)
            {
                //TODO: Have a GUI class that handles show gui bits and whatever
                GUI.DrawTexture(new Rect(Screen.width - 64, Screen.height - 64, 32, 32), _dismissalPromptGraphic, ScaleMode.StretchToFill, true, 1.0F);
            }
        }

    }
}
