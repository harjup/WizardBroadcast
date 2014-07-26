using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Assets.Scripts.GameState;
using Assets.Scripts.Interactables;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class UserProgressStore : Singleton<UserProgressStore>
    {

        //Bluhhhhg
        //Maps each scene to a map of TreasureTypes and amounts
        private Dictionary<Scene, Dictionary<TreasureType, int>> _treasureTotals;

        //Add a treasure to the treasure stash

        void Start()
        {
            Init();
        }
        void OnLevelWasLoaded(int level)
        {
            Init();
        }

        void Init()
        {
            //reset persisted progress if we're in start
            if (SceneMap.GetSceneFromStringName(Application.loadedLevelName) == Scene.Start 
                || _treasureTotals == null)
            {
                _treasureTotals =
                    new Dictionary<Scene, Dictionary<TreasureType, int>>()
                    {
                        {Scene.Level1,  new  Dictionary<TreasureType, int>()},
                        {Scene.Level2,  new  Dictionary<TreasureType, int>()},
                        {Scene.Level3,  new  Dictionary<TreasureType, int>()},
                        {Scene.Level4,  new  Dictionary<TreasureType, int>()}
                    };
            }
        }

        public void AddTreasure(TreasureType treasure)
        {
            var currentScene = SceneMap.GetSceneFromStringName(Application.loadedLevelName);

            Debug.Log(String.Format("Adding {0} to {1}", treasure, currentScene));

            if (currentScene == Scene.Undefined)
            {
                return;
            }
            if (!_treasureTotals.ContainsKey(currentScene))
            {
                _treasureTotals.Add(currentScene, new  Dictionary<TreasureType, int>());
            }

            if (!_treasureTotals[currentScene].ContainsKey(treasure))
            {
                _treasureTotals[currentScene].Add(treasure, 1);
                return;
            }

            _treasureTotals[currentScene][treasure] += 1;
        }

        public Dictionary<TreasureType, int> GetTreasureTotals(Scene scene = Scene.Undefined)
        {
            if (scene == Scene.Undefined)
            {
                //return everything smooshed into a single dictionary. Maybe if can just be another grand total dictionary
            }
            return _treasureTotals[scene];
        }


    }
}
