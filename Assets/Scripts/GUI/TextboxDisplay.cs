using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.GUI
{
    public class TextboxDisplay : Singleton<TextboxDisplay>
    {
        private static bool isRunning = false;

        private const int MaxPageLength = 180;
        private string _speaker;
        private string _fullDisplayText;
        private string _currentDisplayText;
        private int _displayIndex = 1;
        private bool _waitingForDismissal = false;
        bool textureBlink = false;

        //The Interaction key may be true the first time update is run after initializing text,
        //Because the button to initialize it is the same one that proceeds text
        //So we need to assume the button is down when DisplayText starts and disregard inputs until it is not
        private bool keyDownFromInit = false;

        void Update()
        {
            if (!isRunning)
                return;

            if (InputManager.Instance.InteractAction && !keyDownFromInit)
            {
                _displayIndex = _fullDisplayText.Length;
                if (_waitingForDismissal)
                {
                    _waitingForDismissal = false;
                }
            }
            else
            {
                keyDownFromInit = false;
            }
        }



        public IEnumerator DisplayText(string text, Action doneCallback)
        {
            yield return StartCoroutine(DisplayText(text, null, doneCallback));
        }

        public IEnumerator DisplayText(string text, string speaker, Action doneCallback)
        {
            _speaker = speaker;

            keyDownFromInit = true;

            if (isRunning)
            {
                Debug.LogError("Textbox display is already running, rejecting display of " + text);
                yield break;
            }

            var linesByLineBreak = text.Split(new[] {"\r\n", "\n", "\t"}, StringSplitOptions.None);

            foreach (var line in linesByLineBreak)
            {
                //If the given string is too long split it into multiple
                var charCount = 0;
                var lines = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .GroupBy(w => (charCount += w.Length + 1) / MaxPageLength)
                .Select(g => string.Join(" ", g.ToArray()));

                isRunning = true;

                foreach (var l in lines)
                {
                    //Initialize text values
                    _fullDisplayText = l;
                    _currentDisplayText = "";

                    yield return StartCoroutine(CrawlText());

                    //Done, do cleanup
                    _fullDisplayText = "";
                    _currentDisplayText = null;
                    _displayIndex = 1;
                }
            }


            

            isRunning = false;
            doneCallback();
        }

        IEnumerator CrawlText()
        {
            while (_displayIndex <= _fullDisplayText.Length)
            {
                _currentDisplayText = _fullDisplayText.Substring(0, _displayIndex);
                _displayIndex += 1;
                yield return new WaitForSeconds(.05f);
            }

            _waitingForDismissal = true;

            //Blink the dismiss texture while waiting for dismissal
            while (_waitingForDismissal)
            {
                textureBlink = !textureBlink;
                yield return new WaitForSeconds(.5f);
            }
        }

        void OnGUI()
        {
            GuiManager.Instance.DrawTextBox(_currentDisplayText, _speaker);
            if (_waitingForDismissal && textureBlink)
            {
                GuiManager.Instance.DrawTextProceedPrompt();
            }
        }




    }
}
