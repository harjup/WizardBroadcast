using System;
using Assets.Common;
using Assets.Scripts.GUI;
using Assets.Scripts.Interactables;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class UserMessenger : ExaminableBase
{
    private UserComment _comment;
    private GameObject playerObject;
    private int _talkCount;

    void Start()
    {
        playerObject = FindObjectOfType<InfoPlayer>().gameObject;
        iTween.MoveTo(gameObject, iTween.Hash("y", transform.position.y + .2f, "looptype", iTween.LoopType.pingPong, "time", 1f, "easetype", iTween.EaseType.easeInOutQuad));
    }

    void Update()
    {
        iTween.LookUpdate(gameObject, iTween.Hash("looktarget", playerObject.transform.position, "axis", "y", "time", 20f));
    }

    public void SetComment(UserComment comment)
    {
        _comment = comment;
        if (comment.WorldPositon == null) comment.WorldPositon = "(52,0,0)";

        transform.position = comment.WorldPositon.ParseToVector3();
    }

    public override IEnumerator Examine(Action callback)
    {
        yield return StartCoroutine(TextboxDisplay.Instance.DisplayText(_comment.Content, _comment.Name, () => { }));
        callback();
        _talkCount++;

        if (_talkCount >= 5) StartCoroutine(FloatAway());
    }

    private bool _floatAwayStarted = false;
    IEnumerator FloatAway()
    {
        if (_floatAwayStarted) yield break;
        _floatAwayStarted = true;

        iTween.MoveTo(gameObject, iTween.Hash("y", transform.position.y + 20f, "time", 20f, "easetype", iTween.EaseType.easeInOutQuad));
        yield return new WaitForSeconds(20f);
        Destroy(gameObject, 2f);
    }

}
