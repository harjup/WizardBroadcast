using System;
using System.Runtime.InteropServices;
using Assets.Scripts.GUI;
using Assets.Scripts.Interactables;
using Assets.Scripts.Managers;
using Assets.Scripts.Repository;
using UnityEngine;
using System.Collections;
using WizardCentralServer.Model.Dtos;

public class TweetMessage : ExaminableBase
{
    private TwitterStatus _status;
    
    void Awake()
    {
        renderer.material.color = Color.grey;
    }

    public TwitterStatus Status
    {
        get { return _status; }
        set
        {
            _status = value;
            renderer.material.color = Color.blue;
        }
    }

    public override IEnumerator Examine(Action callback)
    {
        yield return StartCoroutine(TextboxDisplay.Instance.DisplayText(_status.Text, _status.User.ScreenName, () => { }));
        callback();
    }
}
