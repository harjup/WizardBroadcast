using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    public enum SceneState
    {
        Undefined,
        Closed,
        Open
    }

    /// <summary>
    /// Stores the state of the session, like what levels are open or closed
    /// </summary>
    public static class SessionStateStore
    {
        private static readonly Dictionary<Scene, SceneState> SceneStates = new Dictionary<Scene, SceneState>()
        {
            {Scene.Level1, SceneState.Closed},
            {Scene.Level2, SceneState.Closed},
            {Scene.Level3, SceneState.Closed},
            {Scene.Level4, SceneState.Closed}
        };

        public static void SetSceneState(Scene scene, SceneState state)
        {
            SceneStates[scene] = state;
        }

        public static SceneState GetSceneState(Scene scene)
        {
            return SceneStates[scene];
        }
    }
}
