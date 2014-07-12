using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    //Remembe to leave calls to this in OnGUI so stuff gets drawn right
    class GuiManager : Singleton<GuiManager>
    {
        private Texture _dismissalPromptGraphic;
        private GUIStyle textBoxStyle = new GUIStyle();

        private bool _showInteractionPrompt = false;
        public void DrawInteractionPrompt()
        {
            _showInteractionPrompt = true;
        }

        private string _textBoxContents;
        public void DrawTextBox(string text)
        {
            _textBoxContents = text;
        }

        private bool _showTextProceedPrompt = false;
        public void DrawTextProceedPrompt()
        {
            _showTextProceedPrompt = true;
        }

        private bool _drawGeneralInfo = false;
        public void DrawGeneralInfo()
        {
            _drawGeneralInfo = true;
        }


        void Start()
        {
            _dismissalPromptGraphic = Resources.Load<Texture>("Textures/dismissPrompt");
        }

        void Update()
        {
            StartCoroutine(ResetDrawFlags());
        }

        void OnGUI()
        {
            textBoxStyle.fontSize = Mathf.RoundToInt(Screen.width / 40);
            textBoxStyle.wordWrap = true;

            if (_showInteractionPrompt)
            {
                GUI.DrawTexture(new Rect(Screen.width - 64, Screen.height - 64, 32, 32), _dismissalPromptGraphic, ScaleMode.StretchToFill, true, 1.0F);
            }
            if (_showTextProceedPrompt)
            {
                GUI.DrawTexture(new Rect(Screen.width - 64, Screen.height - 64, 32, 32), _dismissalPromptGraphic, ScaleMode.StretchToFill, true, 1.0F);
            }
            if (_textBoxContents != null)
            {
                GUI.Box(new Rect(Screen.width / 12f, Screen.height / 1.2f, Screen.width / 1.2f, Screen.height / 7.5f), _textBoxContents, textBoxStyle);
            }
            if (_drawGeneralInfo)
            {
                var treasureTotals =
                    UserProgressStore.Instance.GetTreasureTotals(
                        SceneMap.GetSceneFromStringName(Application.loadedLevelName));

                string str = "";
                int index = 0;
                foreach (var treasureTotal in treasureTotals)
                {
                    str = treasureTotal.Key + " " + treasureTotal.Value;
                    GUI.TextArea(new Rect((Screen.width / 1.2f), (Screen.height / 12f) + (index * 50f), Screen.width / 12f, Screen.height / 10f), str);
                    index++;
                }
            }
        }

        IEnumerator ResetDrawFlags()
        {
            yield return new WaitForEndOfFrame();
            _drawGeneralInfo = false;
            _showTextProceedPrompt = false;
            _textBoxContents = null;
            _showInteractionPrompt = false;
        }
    }
}
