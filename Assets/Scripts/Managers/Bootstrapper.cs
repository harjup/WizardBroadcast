using System;
using System.Collections.Generic;
using Assets.Scripts.Player;
using UnityEngine;

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


        //TODO: Determining a decent method of retreiving prefabs source paths. Manually setting them as public gameobject properties sounds awful
        
        private Dictionary<Type, string> _prefabsToSpawn = new Dictionary<Type, string>()
        {
            {typeof(InfoPlayer), @"Prefabs/PlayerCharacter"},
            {typeof(ScheduleTracker), @"Prefabs/ScheduleTracker"},
            {typeof(TimeTracker), @"Prefabs/TimeTracker"}
        };

        void Start()
        {
            foreach (var prefabToSpawn in _prefabsToSpawn)
            {
                if (!ObjectOfTypeExistsInScene(prefabToSpawn.Key))
                {
                    var createdGameObject = Instantiate(Resources.Load(prefabToSpawn.Value, typeof(GameObject)), Vector3.zero, new Quaternion()) as GameObject;

                    //Anything created by the bootstrap should persist between scenes
                    if (createdGameObject != null)
                    {
                        DontDestroyOnLoad(createdGameObject.transform);
                    }
                }   
            }
        }


    }
}
