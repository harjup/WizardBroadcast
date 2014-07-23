using System;
using Assets.Common;
using Assets.Scripts.GUI;
using Assets.Scripts.Interactables;
using UnityEngine;
using System.Collections;

public class UserMessenger : ExaminableBase
{
    private UserComment _comment;

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
    }
}
