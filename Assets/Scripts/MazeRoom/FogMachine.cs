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
            SetFogForForMaze();
            return;    
        }
        RenderSettings.fog = false;
    }

    public void SetFogForForMaze()
    {
        RenderSettings.fogColor = Color.gray;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = .05f;
        RenderSettings.fog = true;
        
    }

    public void SetFogForIndoors()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 20f;
        RenderSettings.fogEndDistance = 30f;
    }

    public void DisableFog()
    {
        RenderSettings.fog = false;
    }
}
