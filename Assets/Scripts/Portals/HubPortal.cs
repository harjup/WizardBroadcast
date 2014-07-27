﻿using System.Collections;
using Assets.Scripts.GameState;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Portals
{
    public class HubPortal : MonoBehaviour
    {
        private Scene sceneToLoad = Scene.Hub;
        private Scene currentScene = Scene.Undefined;
        private bool isActive = true;

        void Start()
        {
            ScheduleTracker.levelActivated += OnLevelActivate;

            
            //Check and set the current state for the Level.
            currentScene = SceneMap.GetSceneFromStringName(Application.loadedLevelName);
            var state = SessionStateStore.GetSceneState(currentScene);
            OnLevelActivate(sceneToLoad, state);
        }

        //Runs when the next scene is loaded, game exited, object destroyed, etc
        void OnDisable()
        {
            ScheduleTracker.levelActivated -= OnLevelActivate;
        }

        /// <summary>
        /// Recieve Event to change the current level portal's state. This will have a transition cause the player is present.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="state"></param>
        void OnLevelActivate(Scene scene, State state)
        {
            if (currentScene == scene
                && state == State.InActive)
            {
                StartCoroutine(LeaveLevelTransition("has left"));
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<InfoPlayer>() != null && isActive)
            {
                StartCoroutine(LeaveLevelTransition("completed"));
            }
        }


        IEnumerator LeaveLevelTransition(string leaveText)
        {
            SignalrEndpoint.Instance.Broadcast(GuiManager.Instance.PlayerNameInput 
                + " " + leaveText + " "  
                + SceneMap.DescriptiveName(SceneMap.GetSceneFromStringName(Application.loadedLevelName)) 
                + "!");

            InputManager.Instance.PlayerInputEnabled = false;
            yield return StartCoroutine(CameraManager.Instance.DoWipeOut(.5f));
            yield return new WaitForSeconds(.5f);
            StartCoroutine(CameraManager.Instance.DoWipeIn(.5f));
            Application.LoadLevel(SceneMap.GetScene(sceneToLoad));
            InputManager.Instance.PlayerInputEnabled = true;
        }

    }
}
