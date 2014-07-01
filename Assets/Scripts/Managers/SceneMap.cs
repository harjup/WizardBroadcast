using UnityEngine;
using System.Collections;


//TODO: Map out the string versions of scene names to this enum, so we can just use that
public class SceneMap : Singleton<SceneMap> 
{
    public enum sceneList
    {
        undefined,
        level1,
        level2,
        level3,
        level4
    }
    public string GetScene(sceneList scene)
    {
        return "level1";
    }
}
