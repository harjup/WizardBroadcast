using UnityEngine;
using System.Collections;

public class SquishAnimIce : MonoBehaviour, ISquishAnim
{
    private Vector3 initialRotation;
    private IEnumerator _idleRoutine;
    private GameObject _treasurePrefab;

    void Start()
    {
        initialRotation = transform.eulerAngles;
        _idleRoutine = IdleAnim();
        StartCoroutine(_idleRoutine);
        _treasurePrefab = Resources.Load("Prefabs/LittleTreasure") as GameObject;
    }


    IEnumerator IdleAnim()
    {
        while (true)
        {
            iTween.RotateTo(gameObject, iTween.Hash("z", 15f, "time", .1f, "looptype", iTween.LoopType.none, "easetype", iTween.EaseType.easeInOutExpo));
            yield return new WaitForSeconds(.6f);
            iTween.RotateTo(gameObject, iTween.Hash("z", -15f, "time", .1f, "looptype", iTween.LoopType.none, "easetype", iTween.EaseType.easeInOutExpo));
            yield return new WaitForSeconds(.6f);
        }
    }


    public void OnSquish(Vector3 position)
    {
        StopCoroutine(_idleRoutine);
        iTween.Stop(gameObject);
        transform.eulerAngles = initialRotation;
        iTween.RotateTo(gameObject, iTween.Hash("x", -90f, "time", .1f, "looptype", iTween.LoopType.none, "easetype", iTween.EaseType.easeInOutExpo));
        
        //Play sound effect
        //Instantiate(_treasurePrefab, position.SetY(2f), Quaternion.identity);
    }
}
