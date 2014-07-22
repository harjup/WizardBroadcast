using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    void Start()
    {
        iTween.ShakePosition(gameObject, iTween.Hash("amount", Vector3.left/4f, "looptype", iTween.LoopType.loop, "delay", 0f));
    }

}
