using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.Interactables
{
    public class TextBag : MonoBehaviourBase
    {
        public string id;
        public string text;

        public TextAction GetAction()
        {
            //TODO: Maybe return an empty action or something if it doesn't exist
            return GetComponent<TextAction>();
        }
    }
}