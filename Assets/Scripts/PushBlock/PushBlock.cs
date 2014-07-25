using System;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;

using UnityEngine;
using System.Collections;

public class PushBlock : MonoBehaviourBase
{
    public bool isSlippery = false;
    public const float StandardPushDistance = 2.5f;

    private GameObject _tweenTarget;

    private bool _cleaningUp = false;
    public bool CleaningUp
    {
        get { return _cleaningUp; }
        private set { _cleaningUp = value; }
    }

    private static bool settleBlocksStarted = false;

    void Awake()
    {
        _tweenTarget = transform.parent.gameObject;
    }

    void Start()
    {
        //Shittly singleton implementation to ensure this is only called once
        if (settleBlocksStarted) return;
        settleBlocksStarted = true;
        StartCoroutine(SettleBlocks());
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

        StartCoroutine(SettleBlocks());
        collider.enabled = true;
        callback(disengagePlayer);
    }


    public IEnumerator SettleBlocks()
    {
        var blockMoved = true;
        while (blockMoved)
        {
            blockMoved = false;
            var blocks = FindObjectsOfType<PushBlock>().Where((b) => !b._cleaningUp);
            foreach (var pushBlock in blocks)
            {
                if (pushBlock == null) continue;
                yield return StartCoroutine(pushBlock.ApplyGravity((b) => { if (b) { blockMoved = true; } }));
            }
        }
    }

    public IEnumerator ApplyGravity(Action<bool> callback)
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, Vector3.down, 100);
        float leastDistance = 100;
        foreach (var raycastHit in hits)
        {
            if (raycastHit.transform.GetComponent<InfoPlayer>() != null) continue;

            var edgePosition = (transform.position.y - ((transform.localScale.y / 2f)));
            var pointDifference = edgePosition - (raycastHit.point.y);
            if (pointDifference < leastDistance)
            {
                leastDistance = pointDifference;
            }
        }
        if (leastDistance > .01f && leastDistance <= 100)
        {
            iTween.MoveTo(_tweenTarget, iTween.Hash(
            "y", (transform.position.y - leastDistance),
            "time", .5f,
            "easetype", iTween.EaseType.easeInBack));
            yield return new WaitForSeconds(0.6f);
            callback(true);
            yield break;
        }
        callback(false);
    }

    public bool TopBlocked()
    {
        return transform.parent.GetComponentInChildren<BlockTop>().IsBlocked;
    }

    public IEnumerator Die()
    {
        _cleaningUp = true;
        iTween.ScaleTo(_tweenTarget, Vector3.zero, .5f);
        yield return new WaitForSeconds(.5f);
        iTween.Stop(_tweenTarget);
        Destroy(_tweenTarget, 1f);
    }
}
