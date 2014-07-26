using UnityEngine;
using System.Collections;

public class SquishAnim : MonoBehaviour
{
    private Vector3 initialPosition;
    private IEnumerator _jumpRoutine;

    void Start()
    {
        initialPosition = transform.position;
        _jumpRoutine = JumpAnim();
        StartCoroutine(_jumpRoutine);
    }


    IEnumerator JumpAnim()
    {
        while (true)
        {
            iTween.MoveTo(gameObject, iTween.Hash("y", initialPosition.y + 3f, "time", .5f, "looptype", iTween.LoopType.none, "easetype", iTween.EaseType.easeOutCubic));
            yield return new WaitForSeconds(.6f);
            iTween.MoveTo(gameObject, iTween.Hash("y", initialPosition.y, "time", .5f, "looptype", iTween.LoopType.none, "easetype", iTween.EaseType.easeInCubic));
            yield return new WaitForSeconds(1f);
        }
        
    }


    public void OnSquish()
    {
        StopCoroutine(_jumpRoutine);
        iTween.Stop(gameObject);
        transform.position = initialPosition;
        //Play sound effect
    }
}
