using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    public enum State
    {
        Undefined,
        InActive,
        Active
    }

    /// <summary>
    /// Stores the state of the session, like what levels are open or closed
    /// </summary>
    public static class SessionStateStore
    {
        //Identifier to know which ghost is the player's
        private static string _playerId;

        private static bool _scheduleTrackerInitialized;

        private static readonly Dictionary<Scene, State> SceneStates = new Dictionary<Scene, State>()
        {
            {Scene.Level1, State.InActive},
            {Scene.Level2, State.InActive},
            {Scene.Level3, State.InActive},
            {Scene.Level4, State.InActive}
        };


        public static bool IsScheduleTrackerInitialized()
        {
            return _scheduleTrackerInitialized;
        }

        public static void SetScheduleTrackInit()
        {
            _scheduleTrackerInitialized = true;
        }

        public static string PlayerId
        {
            get { return _playerId; }
            set { _playerId = value; }
        }


        

        public static void SetSceneState(Scene scene, State state)
        {
            SceneStates[scene] = state;
        }

        public static State GetSceneState(Scene scene)
        {
            return SceneStates[scene];
        }

        /// <summary>
        /// Returns the current scene that's set to active.
        /// If there are multiple active scenes it return the first
        /// If there are no active scenes it returns the hub
        /// </summary>
        /// <returns></returns>
        public static Scene GetActiveScene()
        {
            foreach (var sceneState in SceneStates)
            {
                if (sceneState.Value == State.Active)
                {
                    return sceneState.Key;
                }
            }
            return Scene.Hub;
        }
    }
}
