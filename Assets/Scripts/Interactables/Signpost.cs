using System;
using System.Collections;

namespace Assets.Scripts.Interactables
{
    public class Signpost : MonoBehaviourBase, IExaminable
    {
        //TODO: Get this text from *somewhere*
        //TODO: Be able to chain multiple textboxes together. Maybe make this a list and iterate through each one
        private const string textToShow = "Hello, I am a basic signpost!";

        public IEnumerable Examine(Action callback)
        {
            yield return TextboxDisplay.Instance.DisplayText(textToShow, callback);
        }
    }
}