using UnityEngine;
using System.Collections;

public class SquishAnim : MonoBehaviour
{
    void Start()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", transform.position.y + 3f, "time", 1f, "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.easeInBack));
    }

    void OnSquish()
    {
        
    }
}
