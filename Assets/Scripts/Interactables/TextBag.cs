using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.Interactables
{
    public class TextBag : MonoBehaviourBase
    {
        public string id;
        public string flag;
        public string Name;
        public string text;

        public void ExecuteAction()
        {
            foreach (var textAction in GetComponents<TextAction>())
            {
                textAction.Execute();
            }

        }
    }
}