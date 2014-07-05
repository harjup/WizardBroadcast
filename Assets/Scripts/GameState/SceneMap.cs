using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    public enum Scene
    {
        Undefined,
        Level1,
        Level2,
        Level3,
        Level4,
        Hub
    }

    //TODO: Figure out what folder to shove this in and what to name it
    //Singleton service for mapping enum values to scene names
    public static class SceneMap
    {
        

        //TODO: Give these scenes descriptive names!
        private static readonly Dictionary<Scene, string> SceneDictionary = new Dictionary<Scene, string>()
        {
            {Scene.Level1, "level1"},
            {Scene.Level2, "level2"},
            {Scene.Level3, "level3"},
            {Scene.Level4, "level4"},
            {Scene.Hub, "HubZone"},
        };


        public static string GetScene(Scene scene)
        {
            if (scene == Scene.Undefined)
            {
                Debug.LogError("Tried load a Scene using an uninitialized Scene enum");
            }

            return SceneDictionary[scene];
        }
    }
}
