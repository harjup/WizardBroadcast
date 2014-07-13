using Assets.Scripts.GameState;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Collections;

public class SetFlag : TextAction
{
    public string flag;
    public bool value = true;

    public override void Execute()
    {
        var currentScene = SceneMap.GetSceneFromStringName(Application.loadedLevelName);
        EventFlagStore.SetFlag(currentScene, flag, value);
    }
}
