using Assets.Scripts.GameState;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Portals
{
    public class HubPortal : MonoBehaviour
    {
        private Scene sceneToLoad = Scene.Hub;
        private bool isActive = true;

        void Start()
        {

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
