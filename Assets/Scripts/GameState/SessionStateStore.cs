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
        private static readonly Dictionary<Scene, State> SceneStates = new Dictionary<Scene, State>()
        {
            {Scene.Level1, State.InActive},
            {Scene.Level2, State.InActive},
            {Scene.Level3, State.InActive},
            {Scene.Level4, State.InActive}
        };

        public static void SetSceneState(Scene scene, State state)
        {
            SceneStates[scene] = state;
        }

        public static State GetSceneState(Scene scene)
        {
            return SceneStates[scene];
        }
    }
}
