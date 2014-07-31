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
        //renderer.material.color = Color.grey;
    }

    public TwitterStatus Status
    {
        get { return _status; }
        set
        {
            _status = value;
            //renderer.material.color = Color.blue;
            iTween.MoveTo(gameObject, iTween.Hash("y", transform.position.y + .2f, "looptype", iTween.LoopType.pingPong, "time", 1f, "easetype", iTween.EaseType.easeInOutQuad));
            transform.eulerAngles = transform.eulerAngles.SetZ(-5f);
            iTween.RotateTo(gameObject, iTween.Hash("z", 5f, "looptype", iTween.LoopType.pingPong, "time", 2f, "easetype", iTween.EaseType.easeInOutQuad));
        }
    }

    public override IEnumerator Examine(Action callback)
    {
        yield return StartCoroutine(TextboxDisplay.Instance.DisplayText(_status.Text, _status.User.ScreenName, () => { }));
        callback();
    }
}
