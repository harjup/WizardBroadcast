using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Player;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Ensures all the objects to make the game work ahve been created for a given scene
    /// Could use a better name, maybe another word like SceneBootstrapper I dunno
    /// </summary>
    class Bootstrapper : MonoBehaviourBase
    {
        //TODO: Experiment with this sneaky idea
        //Here's a sneaky idea, what if we embedded this logic in every monoBehaviourBase as a singleton method? 
        //It might be terrible or great!!!

        void Start()
        {
            var player = FindObjectsOfInterface<InfoPlayer>();
            if (player.Count == 0)
            {
                //TODO: Determining a decent method of retreiving prefabs source paths. Manually setting them as public gameobject properties sounds awful
                //TODO: Get a better method of determining the position to drop the player at. Maybe a spawnpoint or somethin.
                Instantiate(Resources.Load(@"Prefabs/PlayerCharacter", typeof(GameObject)), new Vector3(0f, 2.5f, 0f), new Quaternion());
            }
        }


    }
}
