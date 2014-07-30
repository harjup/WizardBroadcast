using System;
using Assets.Scripts.GameState;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections.Generic;

namespace WizardBroadcast
{
    public class LevelPortal : MonoBehaviourBase
    {
        private Texture2D _defaultTexture;
        private Texture2D _activeTexture;
        private GameObject mesh;

        public Scene sceneToLoad;
        private bool isActive = false;

        void Start()
        {
            _defaultTexture = Resources.Load("HubTeleporter/Hub Teleporter Texture") as Texture2D;
            _activeTexture = Resources.Load("HubTeleporter/Hub Teleporter Texture Active") as Texture2D;

            //TODO: Wooo, specific heirarchy traversal this is shit!!!!
            mesh = transform.GetChild(0).GetChild(0).gameObject;

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
                        mesh.renderer.material.mainTexture = _defaultTexture;
                        //gameObject.renderer.material.color = Color.red;
                        iTween.Stop(gameObject);
                        isActive = false;
                        break;
                    case State.Active:
                        mesh.renderer.material.mainTexture = _activeTexture;
                        iTween.ShakePosition(gameObject, iTween.Hash("amount", Vector3.forward/4f, "time", .5f, "looptype", iTween.LoopType.loop));
                        iTween.ShakeRotation(gameObject, iTween.Hash("z", 5f, "time", .5f, "looptype", iTween.LoopType.loop));
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
                SignalrEndpoint.Instance.Broadcast(GuiManager.Instance.PlayerNameInput
                + " has entered "
                + SceneMap.DescriptiveName(sceneToLoad)
                + "!");

                //Stop broadcasting ghost stuff when leaving the hub
                //TODO: Is this the best place for this logic etc etc
                SignalrEndpoint.Instance.StopGhost();
	            Application.LoadLevel(SceneMap.GetScene(sceneToLoad));
	        }
	    }

    }
}