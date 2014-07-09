using System;
using System.Collections.Generic;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Ensures all the objects to make the game work ahve been created for a given Scene
    /// This should be used for spawning objects that are supposed to persist between scenes
    /// </summary>
    class Bootstrapper : MonoBehaviourBase
    {
        //TODO: Experiment with this sneaky idea
        //Here's a sneaky idea, what if we embedded this logic in every monoBehaviourBase as a singleton method? 
        //It might be terrible or great!!! <-- (2-3 days later: I think this not good that is way too race conditiony)

        enum PrefabSet
        {
            Undefined,
            Player,
            Manager
        }

        private bool playerPrefabsSpawned = false;
        private readonly Dictionary<Type, string> _playerPrefabs = new Dictionary<Type, string>()
        {
            //{typeof(InfoPlayer), @"Prefabs/PlayerCharacter"}
            {typeof(InfoPlayer), @"Prefabs/Player_Alt"}
        };

        //TODO: Make this a dictionary for Singeltons of T and just check if the Instance is null
        private bool managerPrefabsSpawned = false;
        private readonly Dictionary<Type, string> _managerPrefabs = new Dictionary<Type, string>()
        {
            {typeof(ScheduleTracker), @"Prefabs/ScheduleTracker"},
            {typeof(TimeTracker), @"Prefabs/TimeTracker"},
            {typeof(TweetsManager), @"Prefabs/TweetsManager"},
            {typeof(CommentsManager), @"Prefabs/CommentsManager"},

            {typeof(SignalrEndpoint), @"Prefabs/SignalrEndpoint"},
            {typeof(PeerTracker), @"Prefabs/PeerTracker"},
            {typeof(TextboxDisplay), @"Prefabs/TextboxDisplay"}
        };


        // I want init logic to run on the start scene and any scenes loaded after so I need an init
        // method that's called on both Start and OnLevelWasLoaded
        void Awake()
        {
            Init();
        }
        void OnLevelWasLoaded(int level)
        {
            Init();
        }

        void Init()
        {
            //Only ensure the player has been spawned if we're not in the start scene
            if (Application.loadedLevelName != SceneMap.GetScene(Scene.Start))
            {
                SpawnPrefabs(PrefabSet.Player);
            }
            //Make sure managers are present no matter what
            SpawnPrefabs(PrefabSet.Manager);
        }
        
        void SpawnPrefabs(PrefabSet set)
        {
            Dictionary<Type, string> prefabSet = null;
            var rootName = "Undefined";
            switch (set)
            {
                case PrefabSet.Undefined:
                    Debug.LogError("Tried to spawn an undefined prefab set");
                    break;
                case PrefabSet.Player:
                    prefabSet = _playerPrefabs;
                    rootName = "Player";
                    break;
                case PrefabSet.Manager:
                    prefabSet = _managerPrefabs;
                    rootName = "Managers";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("set");
            }

            var root = FindRootByName(rootName);

            foreach (var prefabToSpawn in prefabSet)
            {
                if (!ObjectOfTypeExistsInScene(prefabToSpawn.Key))
                {
                    var createdGameObject = Instantiate(Resources.Load(prefabToSpawn.Value, typeof(GameObject)), Vector3.zero, new Quaternion()) as GameObject;
                    Debug.Log("Creating: " + createdGameObject.name);
                    //Anything created by the bootstrapper should persist between scenes
                    if (createdGameObject != null)
                    {
                        createdGameObject.transform.parent = root.transform;
                        DontDestroyOnLoad(createdGameObject);
                    }
                }
            }
        }

        GameObject FindRootByName(string rootName)
        {
            var managerRoot = GameObject.Find(rootName) ?? new GameObject();
            managerRoot.name = rootName;
            DontDestroyOnLoad(managerRoot);
            return managerRoot;
        }
    }
}
