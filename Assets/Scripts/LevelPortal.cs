using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

namespace WizardBroadcast
{
    public class LevelPortal : MonoBehaviourBase
    {

        public SceneMap.scene sceneToLoad;

	    void OnTriggerEnter(Collider other)
	    {
	        if (other.GetComponent<InfoPlayer>() != null)
	        {
	            Application.LoadLevel(SceneMap.GetScene(sceneToLoad));
	        }
	    }
	
	
    }
}