using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GUI;
using Assets.Scripts.Repository;

namespace Assets.Scripts.Interactables
{
    public class Signpost : ExaminableBase
    {
        public string scriptId = "";
        private List<TextBag> textBags;
        void Start()
        {
            textBags = GetComponentsInChildren<TextBag>().ToList();
            for (int i = 0; i < textBags.Count; i++)
            {
                var id = textBags[i].id;
                //If the id is not explicitly specified, set it to a default value
                if (String.IsNullOrEmpty(id))
                {
                    id = (i + 1).ToString("D2"); //Its place in the list with a padded zero. EX: "06", "15"
                    textBags[i].id = id;
                }
                textBags[i].text = DialogRepository.Instance.GetScript(scriptId, id);
            }
        }

        private List<string> textbitsToShow = new List<string>()
        {
            "Hello, I am a basic signpost!",
            "Here is textbit number two!",
            "I'll put a third one in for good measure."
        };

        public override IEnumerator Examine(Action callback)
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