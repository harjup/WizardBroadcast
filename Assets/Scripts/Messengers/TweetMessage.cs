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
    private TwitterStatus status;
    
    void Start()
    {
        renderer.material.color = Color.grey;

        TweetsManager.Instance.GetFirstStatus(s =>
        {
            status = s;
            renderer.material.color = Color.cyan;
        }
    );
    }

    public override IEnumerator Examine(Action callback)
    {
        yield return StartCoroutine(TextboxDisplay.Instance.DisplayText(status.Text, () => { }));
    }
}
