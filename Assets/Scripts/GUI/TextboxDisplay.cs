using System;
using System.Net.Mime;
using System.Runtime.InteropServices;
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


    //Gross
    private bool keyIsDown = false;

    void Update()
    {
        //TODO: Input manager so it knows we're holder the key down
        if (Input.GetKeyDown(KeyCode.Z) && ! keyIsDown)
        {
            keyIsDown = true;
            //TODO: Reenable text skipping once we have an input manager
            _displayIndex = _fullDisplayText.Length;
            if (_waitingForDismissal)
            {
                _waitingForDismissal = false;
            }
        }
        else
        {
            keyIsDown = false;
        }
    }

    public IEnumerator DisplayText(string text, Action doneCallback)
    {
        keyIsDown = true; //TODO: Get this out of here

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
