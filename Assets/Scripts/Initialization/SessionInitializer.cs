using System.Collections;
using Assets.Scripts.GameState;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Initialization
{
    /// <summary>
    /// Waits for the the nessesary managers to initialize, then loads the active scene
    /// </summary>
    public class SessionInitializer : MonoBehaviourBase
    {

        // Use this for initialization
        void Start()
        {
            StartCoroutine(WaitForScheduleTrackerToInitialize());
        }

        public IEnumerator WaitForScheduleTrackerToInitialize()
        {
            //Wait for the schedule tracker to set itself up before we try to load up the current level
            while (!SessionStateStore.IsScheduleTrackerInitialized())
            {
                yield return null;
            }
            LoadActiveLevel();
        }

        void LoadActiveLevel()
        {
            var activeScene = SessionStateStore.GetActiveScene();
            var activeSceneName = SceneMap.GetScene(activeScene);
            Debug.Log(activeSceneName);
            Application.LoadLevel(activeSceneName);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
