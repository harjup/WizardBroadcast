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
        }

        //Runs when the next scene is loaded, game exited, object destroyed, etc
        void OnDisable()
        {
            ScheduleTracker.levelActivated -= OnLevelActivate;
        }


        void OnLevelActivate(Scene targetScene)
        {
            if (targetScene == sceneToLoad)
            {
                gameObject.renderer.material.color = Color.green;
                isActive = true;
            }
            else
            {
                gameObject.renderer.material.color = Color.red;
                isActive = false;
            }
        }

	    void OnTriggerEnter(Collider other)
	    {
            if (other.GetComponent<InfoPlayer>() != null && isActive)
	        {
	            Application.LoadLevel(SceneMap.GetScene(sceneToLoad));
	        }
	    }

    }
}