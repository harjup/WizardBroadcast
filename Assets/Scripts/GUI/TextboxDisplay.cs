using System;
using System.Net.Mime;
using System.Runtime.InteropServices;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Collections;

public class TextboxDisplay : Singleton<TextboxDisplay>
{
    private static bool isRunning = false;

    private GUIStyle textBoxStyle = new GUIStyle();

    private string _fullDisplayText;
    private string _currentDisplayText;
    private int _displayIndex = 1;
    private bool _waitingForDismissal = false;
    bool textureBlink = false;
    private Texture dismissalPromptGraphic;


    void Start()
    {
        dismissalPromptGraphic = Resources.Load<Texture>("Textures/dismissPrompt");
    }


    //The Interaction key may be true the first time update is run after initializing text,
    //Because the button to initialize it is the same one that proceeds text
    //So we need to assume the button is down when DisplayText starts and disregard inputs until it is not
    private bool keyDownFromInit = false;

    void Update()
    {
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
        keyDownFromInit = true;

        if (isRunning)
        {
            Debug.LogError("Textbox display is already running, rejecting display of " + text);
            yield break;
        }

        isRunning = true;
        //Initialize text values
        _fullDisplayText = text;
        _currentDisplayText = "";

        yield return StartCoroutine( CrawlText() );

        //Done, do cleanup
        _fullDisplayText = "";
        _currentDisplayText = "";
        _displayIndex = 1;

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
        textBoxStyle.fontSize = Mathf.RoundToInt(Screen.width / 40);
        textBoxStyle.wordWrap = true;
        
        GUI.Box(new Rect(Screen.width / 12f, Screen.height / 1.2f, Screen.width / 1.2f, Screen.height / 7.5f), _currentDisplayText, textBoxStyle);

        if (_waitingForDismissal && textureBlink)
        {
            GUI.DrawTexture(new Rect(Screen.width - 64, Screen.height - 64, 32, 32), dismissalPromptGraphic, ScaleMode.StretchToFill, true, 1.0F);
        }
    }




}
