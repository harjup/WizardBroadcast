using System;
using Assets.Scripts.GameState;
using UnityEngine;
using System.Collections;

public class FogMachine : Singleton<FogMachine>
{
    void Start()
    {
        SetFogForLevel();
    }

    void OnLevelWasLoaded(int level)
    {
        SetFogForLevel();
    }

    void SetFogForLevel()
    {
        Scene scene = SceneMap.GetSceneFromStringName(Application.loadedLevelName);

        if (scene == Scene.Level3)
        {
            RenderSettings.fog = true;
            return;
        }
        RenderSettings.fog = false;
    }
}
