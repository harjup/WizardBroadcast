﻿using System.Collections;
using Assets.Scripts.GameState;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Initialization
{
    /// <summary>
    /// Waits for the the nessesary managers to initialize, then loads the active scene
    /// </summary>
    public class SessionInitializer : MonoBehaviourBase
    {

        private Camera _startCamera;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(WaitForScheduleTrackerToInitialize());
            //CameraManager.Instance.SetMainCamera(_startCamera);

            if (FindObjectOfType<InfoPlayer>() != null)
            {
                FindObjectOfType<InfoPlayer>().gameObject.SetActive(false);
            }

            EventFlagStore.ClearFlags();
        }

        public IEnumerator WaitForScheduleTrackerToInitialize()
        {
            //Wait for the schedule tracker to set itself up before we try to load up the current level
            while (!SessionStateStore.IsScheduleTrackerInitialized())
            {
                yield return new WaitForSeconds(1f);
            }
            LoadActiveLevel();
        }

        //Runs when the next scene is loaded, game exited, object destroyed, etc
        void OnDisable()
        {
            ScheduleTracker.levelActivated -= OnLevelActivate;
        }

        void LoadActiveLevel()
        {
            var activeScene = SessionStateStore.GetActiveScene();

            if (activeScene == Scene.Start)
            {
                ScheduleTracker.levelActivated += OnLevelActivate;
                return;
            }
            //var activeSceneName = SceneMap.GetScene(activeScene);
            //Application.LoadLevel(activeSceneName);
            StartCoroutine(StartGameTransition());
        }

        void OnLevelActivate(Scene scene, State state)
        {
            if (scene == Scene.Hub && state == State.Active)
            {
                StartCoroutine(StartGameTransition());
            }
        }

        private IEnumerator StartGameTransition()
        {
            if (FindObjectOfType<InfoPlayer>() != null)
            {
                FindObjectOfType<InfoPlayer>().gameObject.SetActive(false);
            }

            UserProgressStore.Instance.Init();
            yield return StartCoroutine(CameraManager.Instance.DoWipeOut(.5f));
            yield return new WaitForSeconds(.5f);
            StartCoroutine(CameraManager.Instance.DoWipeIn(.5f));
            Application.LoadLevel(SceneMap.GetScene(Scene.Hub));
        }
    }
}
