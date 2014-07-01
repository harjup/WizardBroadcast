using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

namespace WizardBroadcast
{
    public class LevelPortal : MonoBehaviourBase
    {

        public SceneMap.sceneList sceneToLoad;

	    void OnTriggerEnter(Collider other)
	    {
	        if (other.GetComponent<InfoPlayer>() != null)
	        {
	            Application.LoadLevel(SceneMap.Instance.GetScene(sceneToLoad));
	        }
	    }
	
	
    }
}