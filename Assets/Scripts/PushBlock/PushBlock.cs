﻿using System;
using Assets.Scripts.Managers;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using System.Collections;

public class PushBlock : MonoBehaviour
{

   // private Vector3 _pushDirection = new Vector3(0,0,1);

    private GameObject _tweenTarget;

    void Start()
    {
        _tweenTarget = transform.parent.gameObject;
    }

    string GetAxis(Vector3 pushDirection)
    {
        if (Math.Abs(pushDirection.x) > .01) return "x";
        return "z";
    }


    public IEnumerator PushAction(Vector3 pushDirection, Action callback)
    {
        //ITween from the current position in the given direction until we hit the next x/y coordinate increment (half a block or sommin)
        //TODO So if we have blocks of width 2 and we're facing +x, move to x+1
        //Execute callback when done so we know we can do other things
        var pushDistance = 2f;
        collider.enabled = false;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, pushDirection, 10);
        foreach (var raycastHit in hits)
        {
            if (raycastHit.collider.name == "StopPost")
            {
                var edgePosition = (transform.position + ((transform.localScale / 2f) * pushDirection.Sign()));
                Debug.Log(pushDirection.Sign());
                Debug.Log(transform.position);
                Debug.Log(transform.localScale);
                Debug.Log(pushDirection);
                Debug.Log(raycastHit.point);
                var pointDifference =  edgePosition - (raycastHit.point);
                
                var differenceMagnitude = Vector3.Scale(pointDifference, pushDirection).magnitude;
                Debug.Log(differenceMagnitude);
                //This shoooould only have one component left over and just find that one
                if (differenceMagnitude < .01 && InputManager.Instance.RawVerticalAxis > 0)
                {
                    iTween.Stop(_tweenTarget);
                    //var positionDifference = raycastHit.transform.position - (transform.localScale / 2f + raycastHit.transform.localScale / 2f);
                    //Debug.Log(positionDifference);
                    //Debug.Log(transform.position);
                    //transform.position = transform.position + Vector3.Scale(positionDifference, pushDirection);
                    //Debug.Log(transform.position);
                    yield return new WaitForSeconds(0.3f);
                    collider.enabled = true;
                    callback();
                    yield break;
                }
                if (differenceMagnitude < pushDistance && InputManager.Instance.RawVerticalAxis > 0)
                {
                    pushDistance = differenceMagnitude;
                }
            }
            
        }

        var pushAmount = pushDirection * pushDistance * InputManager.Instance.RawVerticalAxis;
        iTween.MoveTo(_tweenTarget, iTween.Hash(
            "position", (transform.position + pushAmount),
            "time", .3f,
            "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(0.3f);
        collider.enabled = true;
        callback();
    }


/*
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "StopPost")
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position, pushDirection, 10);
            foreach (var raycastHit in hits)
            {
                if (raycastHit.collider.name == "StopPost")
                {
                    iTween.Stop(gameObject);
                    transform.position = transform.position.SetZ(raycastHit.transform.position.z - (transform.localScale.z/2f + raycastHit.transform.localScale.z/2f));
                }
            }
        }

    }*/
}
