using System.Collections.Generic;
using UnityEngine;


public enum Scene
{
    Undefined,
    Start,
    Hub,
    Level1,
    Level2,
    Level3,
    Level4,
}

namespace Assets.Scripts.GameState
{
    //TODO: Figure out what folder to shove this in and what to name it
    //Singleton service for mapping enum values to scene names
    public static class SceneMap
    {
        private static readonly Dictionary<Scene, string> SceneDictionary = new Dictionary<Scene, string>()
        {
            {Scene.Start,   "Start"},
            {Scene.Hub,     "HubZone"},
            {Scene.Level1,  "LoreForest"},
            {Scene.Level2,  "BlockMountain"},
            {Scene.Level3,  "FoggyMaze"},
            {Scene.Level4,  "BigDesert"}
        };


        public static string GetScene(Scene scene)
        {
            if (scene == Scene.Undefined)
            {
                Debug.LogError("Tried get a Scene using an uninitialized Scene enum");
            }
            if (SceneDictionary[scene] == null)
            {
                Debug.LogError("Tried to get a scene not present in the sceneMap");
            }
            return SceneDictionary[scene];
        }

        public static Scene GetSceneFromStringName(string sceneName)
        {
            foreach (var keyValuePair in SceneDictionary)
            {
                if (keyValuePair.Value == sceneName)
                {
                    return keyValuePair.Key;
                }
                
            }
            Debug.LogError("Tried to get a scene not present in the sceneMap");
            return Scene.Undefined;
        }
    }
}
