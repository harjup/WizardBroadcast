using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WizardAnimate : MonoBehaviour
{

    public enum Type
    {
        None,
        Random,
        Bouncy,
        LeftRight,
        ForwardBack
    }

    public Type AnimType = Type.Bouncy;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(StartAnimation());
    }

    IEnumerator StartAnimation()
    {
        if (AnimType == Type.Random)
        {
            AnimType = (Type)Enum.GetValues(typeof(Type)).GetValue(Random.Range(2, 5));
        }

        yield return new WaitForSeconds(Random.Range(0.1f, 1.1f));
        switch (AnimType)
        {
            case Type.None:
                break;
            case Type.Bouncy:
                iTween.MoveTo(gameObject, iTween.Hash("y", transform.position.y + 1f, "easetype", iTween.EaseType.easeInOutExpo, "time", .5f + Random.Range(0f, .5f), "looptype", iTween.LoopType.pingPong));
                break;
            case Type.LeftRight:
                transform.eulerAngles = transform.eulerAngles.SetX(-15f);
                iTween.RotateTo(gameObject, iTween.Hash("x", 15f, "easetype", iTween.EaseType.easeInOutQuart, "time", .3f + Random.Range(0f, .5f), "looptype", iTween.LoopType.pingPong));
                break;
            case Type.ForwardBack:
                transform.eulerAngles = transform.eulerAngles.SetZ(-15f);
                iTween.RotateTo(gameObject, iTween.Hash("z", 15f, "easetype", iTween.EaseType.easeInOutQuad, "time", .5f + Random.Range(0f, .5f), "looptype", iTween.LoopType.pingPong));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
