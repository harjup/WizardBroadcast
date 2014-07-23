using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Assets.Scripts.GameState;
using Assets.Scripts.GUI;
using Assets.Scripts.Managers;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.Interactables
{
    
    public class Signpost : ExaminableBase
    {
        public string scriptId = "";
        private List<TextBag> _textBags;
        private TextBag _currentTextBag;
        void Start()
        {
            //TODO: Automatically create textbins if one doesn't exist for a given piece of dialog
            _textBags = GetComponentsInChildren<TextBag>().ToList();

            if (_textBags == null)
            {
                return;
            }

            for (var i = 0; i < _textBags.Count; i++)
            {
                var id = _textBags[i].id;
                //If the id is not explicitly specified, set it to a default value
                if (String.IsNullOrEmpty(id))
                {
                    id = (i + 1).ToString("D2"); //Its place in the list with a padded zero. EX: "06", "15"
                    _textBags[i].id = id;
                }

                var dialog = DialogRepository.Instance.GetDialogBit(scriptId, id);

                _textBags[i].text = dialog.Text;
                _textBags[i].Name = dialog.Name;

                //Overwrite the given textbag's flag with the flag from the stored version if it exists
                if (dialog.Flag != null){ _textBags[i].flag = dialog.Flag;}
            }

            _currentTextBag = _textBags.First();
        }

        /*public void NextTextbag()
        {
            var index = _textBags.IndexOf(_currentTextBag);
            if (index < _textBags.Count - 1)
            {
                _currentTextBag = _textBags[index + 1];
            }
        }

        public void SetTextbag(string id)
        {
            foreach (var textBag in _textBags)
            {
                if (textBag.id == id)
                {
                    _currentTextBag = textBag;
                    return;
                }
            }
            Debug.LogError(String.Format("Tried to set textbag on signpost {0} to {1}, which does not exist"));
        }*/
        
        public override IEnumerator Examine(Action callback)
        {
            _currentTextBag = GetCurrentTextBag();
            yield return StartCoroutine(TextboxDisplay.Instance.DisplayText(_currentTextBag.text, _currentTextBag.Name, () => {}));
            _currentTextBag.ExecuteAction();
            callback();
        }

        private TextBag GetCurrentTextBag()
        {
            var currentScene = SceneMap.GetSceneFromStringName(Application.loadedLevelName);

            //Set the current textbag to the first one
            var currentBag = _textBags.First();

            //If one of the other bags has a flag that makes it active, use that one instead
            foreach (var textBag in _textBags)
            {
                //TODO: Consider autosetting the flag to the signId + number if undefined
                if (textBag.flag == null) continue;

                //The ones later in the list have priority over earlier ones
                if (EventFlagStore.GetFlag(currentScene, textBag.flag))
                {
                    currentBag = textBag;
                }
            }

            return currentBag;
        }

    }
}