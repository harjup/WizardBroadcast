using System;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

namespace WizardBroadcast
{
    public class LevelPortal : MonoBehaviourBase
    {

        public Scene sceneToLoad;
        private bool isActive = false;

        void Start()
        {
            ScheduleTracker.levelActivated += OnLevelActivate;

            //Check and set the current state for the Level Portal. There will not be transitions for this one because it's when the scene loads up.
            var state = SessionStateStore.GetSceneState(sceneToLoad);
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
            if (scene == sceneToLoad)
            {
                switch (state)
                {
                    case State.Undefined:
                        break;
                    case State.InActive:
                        gameObject.renderer.material.color = Color.red;
                        isActive = false;
                        break;
                    case State.Active:
                        gameObject.renderer.material.color = Color.green;
                        isActive = true;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                
            }
        }

	    void OnTriggerEnter(Collider other)
	    {
            if (other.GetComponent<InfoPlayer>() != null && isActive)
	        {
                //Stop broadcasting ghost stuff when leaving the hub
                //TODO: Is this the best place for this logic etc etc
                SignalrEndpoint.Instance.StopGhost();
	            Application.LoadLevel(SceneMap.GetScene(sceneToLoad));
	        }
	    }

    }
}