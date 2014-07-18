﻿using System;
using System.Linq;
using Assets.Scripts.Managers;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using System.Collections;

public class PushBlock : MonoBehaviourBase
{
    public bool isSlippery = false;
    public const float StandardPushDistance = 2.5f;

    private GameObject _tweenTarget;

    void Start()
    {
        _tweenTarget = transform.parent.gameObject;
        StartCoroutine(ApplyGravity((b) => { }));
    }

    public IEnumerator PushAction(Vector3 pushDirection, Action<bool> callback)
    {
        iTween.EaseType easeType = isSlippery ? iTween.EaseType.easeInOutExpo : iTween.EaseType.linear;
        bool disengagePlayer = isSlippery;
        pushDirection *= InputManager.Instance.RawVerticalAxis;
        var pushDistance = StandardPushDistance;
        collider.enabled = false;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, pushDirection, 100);
        foreach (var raycastHit in hits)
        {
            //There should probably only be the one wall found
            if (raycastHit.collider.GetComponent<BlockStopper>() != null 
                || raycastHit.collider.GetComponent<PushBlock>() != null)
            {
                var edgePosition = (transform.position + ((transform.localScale / 2f) * pushDirection.Sign()));
                var pointDifference =  edgePosition - (raycastHit.point);
                
                var differenceMagnitude = Vector3.Scale(pointDifference, pushDirection).magnitude;


                if (//The block should not move if:
                    //It is being pushed forward and is very close to a wall
                    (differenceMagnitude < .01 && InputManager.Instance.RawVerticalAxis > 0)
                    //It is being pulled back and the player is too close to the wall behind them
                    || (differenceMagnitude < (StandardPushDistance*1.5f) && InputManager.Instance.RawVerticalAxis < 0)
                    //The player is trying to pull a slippery block
                    || (InputManager.Instance.RawVerticalAxis < 0 && isSlippery))
                {
                    iTween.Stop(_tweenTarget);
                    yield return new WaitForSeconds(0.3f);
                    collider.enabled = true;
                    callback(disengagePlayer);
                    yield break;
                }

                if (//Move the block to touch the wall in front of them if...
                    //They being pushed forward and are less than the current push distance away 
                    (differenceMagnitude < pushDistance && InputManager.Instance.RawVerticalAxis > 0) 
                    //The block is slippery
                    || isSlippery)
                {
                    pushDistance = differenceMagnitude;
                }
            }
            
        }

        var pushAmount = pushDirection * pushDistance;// * InputManager.Instance.RawVerticalAxis;
        iTween.MoveTo(_tweenTarget, iTween.Hash(
            "position", (transform.position + pushAmount),
            "time", .5f,
            "easetype", easeType));
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(ApplyGravity((fellDown) => { disengagePlayer = fellDown; }));

        
        var blockMoved = true;
        while (blockMoved)
        {
            blockMoved = false;
            var blocks = FindObjectsOfType<PushBlock>();
            foreach (var pushBlock in blocks)
            {
                yield return StartCoroutine(pushBlock.ApplyGravity((b) => {if (b){blockMoved = true;}}));
            }
        }
        
        collider.enabled = true;
        callback(disengagePlayer);
    }

    public IEnumerator ApplyGravity(Action<bool> callback)
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, Vector3.down, 100);
        float leastDistance = 100;
        foreach (var raycastHit in hits)
        {
            var edgePosition = (transform.position.y - ((transform.localScale.y / 2f)));
            var pointDifference = edgePosition - (raycastHit.point.y);
            if (pointDifference < leastDistance)
            {
                leastDistance = pointDifference;
            }
        }
        if (leastDistance > .01f && leastDistance <= 100)
        {
            Debug.Log("Tweening");
            iTween.MoveTo(_tweenTarget, iTween.Hash(
            "y", (transform.position.y - leastDistance),
            "time", .5f,
            "easetype", iTween.EaseType.easeInBack));
            yield return new WaitForSeconds(0.5f);
            callback(true);
            yield break;
        }
        callback(false);
    }

    public bool TopBlocked()
    {
        return transform.parent.GetComponentInChildren<BlockTop>().IsBlocked;
    }
}
