using System;
using System.Collections;
using Assets.Scripts.GameState;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections.Generic;

public class HubStateManager : MonoBehaviour
{
    private const Scene _scene = Scene.Hub;

    void Start()
    {
        ScheduleTracker.levelActivated += OnLevelActivate;

        //Check and set the current state for the Level Portal. There will not be transitions for this one because it's when the scene loads up.
        var state = SessionStateStore.GetSceneState(_scene);

        if (TimeTracker.Instance.IsInitialized())
        {
            OnLevelActivate(_scene, state);
        }
    }

    //Runs when the next scene is loaded, game exited, object destroyed, etc
    void OnDisable()
    {
        ScheduleTracker.levelActivated -= OnLevelActivate;
    }

    /// <summary>
    /// Recieve Event to change the current level portal's state. This will have a transition cause the player is present.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="state"></param>
    void OnLevelActivate(Scene scene, State state)
    {
        if (scene == _scene && state == State.InActive)
        {
            StartCoroutine(GameIsOverTransition());
        }
    }

    IEnumerator GameIsOverTransition()
    {
        yield return StartCoroutine(CameraManager.Instance.DoWipeOut(.5f));
        yield return new WaitForSeconds(.5f);
        StartCoroutine(CameraManager.Instance.DoWipeIn(.5f));
        Destroy(GameObject.Find("Player"));
        Application.LoadLevel(SceneMap.GetScene(Scene.Start));
    }
}
