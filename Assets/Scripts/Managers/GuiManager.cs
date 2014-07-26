using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.GameState;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    using UnityEngine;
    //Remember to leave calls to this in OnGUI so stuff gets drawn right
    //This class is a fukin mess I am sorry
    class GuiManager : Singleton<GuiManager>
    {
        private Texture _dismissalPromptGraphic;
        public GUIStyle textBoxStyle;

        private bool _showInteractionPrompt = false;
        public void DrawInteractionPrompt()
        {
            _showInteractionPrompt = true;
        }

        private string _passiveTextBoxContents;
        private string _passiveTextSpeaker;
        public void DrawPassiveTextBox(string text, string speaker)
        {
            _passiveTextBoxContents = text;
            _passiveTextSpeaker = speaker;
        }


        private string _textBoxContents;
        private string _textBoxSpeaker;
        public void DrawTextBox(string text, string speaker)
        {
            _textBoxContents = text;
            _textBoxSpeaker = speaker;
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

        private string _playerNameInput = "Lumpy";
        public string PlayerNameInput { get { return _playerNameInput; } }

        private string _playerMessageInput = "";
        public string PlayerMessageInput
        {
            get
            {
                return _playerMessageInput;
            }
            set
            {
                _playerMessageInput = value;
            }
        }
        private bool _postCommentButtonClicked = false;
        public bool DrawCommentGui = false; //Ugh everything is terrible
        public bool PostComment()
        {
            DrawCommentGui = true;
            if (!_postCommentButtonClicked) return _postCommentButtonClicked;
            _postCommentButtonClicked = false;
            StartCoroutine(KillButtonFlags());
            return true;
        }

        private bool _cancelCommentButtonClicked = false;
        public bool CancelPressed()
        {
            DrawCommentGui = true;
            if (!_cancelCommentButtonClicked) return _cancelCommentButtonClicked;
            _cancelCommentButtonClicked = false;
            StartCoroutine(KillButtonFlags());
            return true;
        }

        //I hate everything
        IEnumerator KillButtonFlags()
        {
            yield return new WaitForEndOfFrame();
            _postCommentButtonClicked = false;
            _cancelCommentButtonClicked = false;
        }


        public bool CommentButtonPressed;

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

            //Player name
            GUI.Label(new Rect(16, 16, 64, 25), "Name:");
            _playerNameInput = GUI.TextField(new Rect(64, 16, 128, 25), _playerNameInput);
                
            if (_playerNameInput.Length > 16)
            {
                _playerNameInput = _playerNameInput.Substring(0, _playerNameInput.Length <= 16 ? _playerNameInput.Length : 20);
            }

            //Comment Input
            if (DrawCommentGui)
            {
                GUI.Box(new Rect(Screen.width/15f, Screen.height/1.5f, Screen.width/2f, Screen.height/16f),
                    "Enter your thoughts and dreams:", textBoxStyle);

                _playerMessageInput =
                    GUI.TextField(
                        new Rect(Screen.width/12f, Screen.height/1.4f, Screen.width/1.2f, Screen.height/7.5f),
                        _playerMessageInput, 180, textBoxStyle);


                if (GUI.Button(new Rect(Screen.width - (64 + 96), Screen.height - 64, 96, 32), "Submit")) 
                    _postCommentButtonClicked = true;


                if (GUI.Button(new Rect(64, Screen.height - 64, 96, 32), "Cancel"))
                    _cancelCommentButtonClicked = true;

                CommentButtonPressed = false;
                return;
            }    
            
            //Shit won't work, putting it directly in comment entry service
            //CommentButtonPressed = GUI.RepeatButton(new Rect(16, 32 + 16, 96, 32), "Comment");
            

            //Interaction button prompts
            if (_showInteractionPrompt)
            {
                GUI.DrawTexture(new Rect(Screen.width - 64, Screen.height - 64, 32, 32), _dismissalPromptGraphic, ScaleMode.StretchToFill, true, 1.0F);
            }
            if (_showTextProceedPrompt)
            {
                GUI.DrawTexture(new Rect(Screen.width - 64, Screen.height - 64, 32, 32), _dismissalPromptGraphic, ScaleMode.StretchToFill, true, 1.0F);
            }

            //Text boxes
            if (_textBoxContents != null)
            {
                GUI.Box(new Rect(Screen.width / 12f, Screen.height / 1.2f, Screen.width / 1.2f, Screen.height / 7.5f), _textBoxContents, textBoxStyle);

                if (_textBoxSpeaker!= null) GUI.Box(new Rect(Screen.width / 15f, Screen.height / 1.3f, Screen.width / 4f, Screen.height / 16f), _textBoxSpeaker, textBoxStyle);
            }
            if (_passiveTextBoxContents != null)
            {
                GUI.Box(new Rect(Screen.width / 15f, Screen.height / 1.6f, Screen.width / 4f, Screen.height / 16f), _passiveTextSpeaker, textBoxStyle);
                GUI.Box(new Rect(Screen.width / 12f, Screen.height / 1.5f, Screen.width / 1.2f, Screen.height / 7.5f), _passiveTextBoxContents, textBoxStyle);
            }

            //Shit score tracking ~thing~
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

            _passiveTextBoxContents = null;
            _passiveTextSpeaker = null;

            _showInteractionPrompt = false;
            DrawCommentGui = false;
        }
    }
}
