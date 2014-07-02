using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    //Singleton service for mapping enum values to scene names
    public static class SceneMap 
    {
        public enum scene
        {
            undefined,
            level1,
            level2,
            level3,
            level4,
            hub
        }

        //TODO: Give these scenes descriptive names!
        private static readonly Dictionary<scene, string> SceneDictionary = new Dictionary<scene, string>()
        {
            {scene.level1, "level1"},
            {scene.level2, "level2"},
            {scene.level3, "level3"},
            {scene.level4, "level4"},
            {scene.hub, "HubZone"},
        };


        public static string GetScene(scene scene)
        {
            if (scene == scene.undefined)
            {
                Debug.LogError("Tried load a scene using an uninitialized scene enum");
            }

            return SceneDictionary[scene];
        }
    }
}
