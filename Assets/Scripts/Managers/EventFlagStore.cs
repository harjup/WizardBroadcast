using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public static class EventFlagStore
    {
        private static Dictionary<Scene, Dictionary<string, bool>> _flagStore = new Dictionary<Scene, Dictionary<string, bool>>();

        public static void SetFlag(Scene scene, string flag, bool value)
        {
            if (!_flagStore.ContainsKey(scene))
            {
                _flagStore.Add(scene, new Dictionary<string, bool>());
            }
            if (!_flagStore[scene].ContainsKey(flag))
            {
                _flagStore[scene].Add(flag, value);
            }
            else
            {
                _flagStore[scene][flag] = value;
            }
        }

        public static bool GetFlag(Scene scene, string flag)
        {
            if (!_flagStore.ContainsKey(scene) || !_flagStore[scene].ContainsKey(flag))
                return false;

            return _flagStore[scene][flag];
        }

        public static void ClearFlags()
        {
            _flagStore = new Dictionary<Scene, Dictionary<string, bool>>();
        }

    }
}
