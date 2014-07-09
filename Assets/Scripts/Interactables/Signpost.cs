using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Interactables
{
    public class Signpost : MonoBehaviourBase, IExaminable
    {
        //TODO: Get this text from *somewhere*
        //TODO: Be able to chain multiple textboxes together. Maybe make this a list and iterate through each one
        private const string textToShow = "Hello, I am a basic signpost!";

        private List<string> textbitsToShow = new List<string>()
        {
            "Hello, I am a basic signpost!",
            "Here is textbit number two!",
            "I'll put a third one in for good measure."
        };

        public IEnumerator Examine(Action callback)
        {
            foreach (var textbit in textbitsToShow)
            {
                //Lets make this callback blank for now, we'll know it's done when we're out of the for loop
                yield return StartCoroutine(TextboxDisplay.Instance.DisplayText(textbit, () => {}));
            }
            callback();
        }
    }
}