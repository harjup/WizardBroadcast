using System;
using System.Linq;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Collections;

//TODO: Refactor this with textbox display to only be one script instead of the two I am making or somin
public class PassiveTextboxDisplay : Singleton<PassiveTextboxDisplay>
{
    private bool isRunning = false;
    private bool breakEarly = false;

    private const int MaxPageLength = 150;
    private string _fullDisplayText;
    private string _currentDisplayText;
    private string _speaker;
    private int _displayIndex = 1;

    private IEnumerator _currentTextCrawl;

    void Start()
    {
        /*StartCoroutine(
            DisplayText("I AM A PASSIVE TEXTBOX< I WILL BE HERE WITH OR WITHOUT YOUR APPROVAL!!!! GO EAT AN APPLE.", "Appleboy",
                () => { }));*/
    }

    //Cleanup between levels
    void OnLevelWasLoaded(int level)
    {
        //Done, do cleanup
        StopAllCoroutines();
        Cleanup();
    }


    public IEnumerator DisplayText(string text, string speaker, Action doneCallback)
    {
        //Wait for any old instances to end and clean up before starting the current one
        if (isRunning)
        {
            StopCoroutine(_currentTextCrawl);
            Cleanup();
        }
        isRunning = true;
        breakEarly = false;
        

        if (!String.IsNullOrEmpty(speaker))
        {
            _speaker = speaker;
        }
        //If the given string is too long split it into multiple
        var charCount = 0;
        var lines = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .GroupBy(w => (charCount += w.Length + 1) / MaxPageLength)
            .Select(g => string.Join(" ", g.ToArray()));

        foreach (var line in lines)
        {
            //Initialize text values
            _fullDisplayText = line;
            _currentDisplayText = "";

            _currentTextCrawl = CrawlText();
            yield return StartCoroutine(_currentTextCrawl);
            //Done, do cleanup
            Cleanup();
        }
        isRunning = false;
        doneCallback();
    }

    void Cleanup()
    {
        _fullDisplayText = "";
        _currentDisplayText = null;
        _speaker = null;

        _displayIndex = 1;
    }

    IEnumerator CrawlText()
    {
        while (_displayIndex <= _fullDisplayText.Length)
        {
            _currentDisplayText = _fullDisplayText.Substring(0, _displayIndex);
            _displayIndex += 1;
            yield return new WaitForSeconds(.025f);
        }
        yield return new WaitForSeconds(3f);
    }

    void OnGUI()
    {
        GuiManager.Instance.DrawPassiveTextBox(_currentDisplayText, _speaker);
    }
}
