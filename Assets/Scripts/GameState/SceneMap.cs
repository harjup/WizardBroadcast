using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
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

    //TODO: Figure out what folder to shove this in and what to name it
    //Singleton service for mapping enum values to scene names
    public static class SceneMap
    {
        //TODO: Give these scenes descriptive names!
        private static readonly Dictionary<Scene, string> SceneDictionary = new Dictionary<Scene, string>()
        {
            {Scene.Start,   "Start"},
            {Scene.Hub,     "HubZone"},
            {Scene.Level1,  "level1"},
            {Scene.Level2,  "level2"},
            {Scene.Level3,  "level3"},
            {Scene.Level4,  "level4"}
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
