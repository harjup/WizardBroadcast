using UnityEngine;
using System.Collections;

public class SquishAnim : MonoBehaviour, ISquishAnim
{
    private Vector3 initialPosition;
    private IEnumerator _jumpRoutine;
    private GameObject _treasurePrefab;

    void Start()
    {
        initialPosition = transform.position;
        _jumpRoutine = JumpAnim();
        StartCoroutine(_jumpRoutine);
        _treasurePrefab = Resources.Load("Prefabs/LittleTreasure") as GameObject;
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


    public void OnSquish(Vector3 position)
    {
        StopCoroutine(_jumpRoutine);
        iTween.Stop(gameObject);
        transform.position = initialPosition;
        //Play sound effect
        //Instantiate(_treasurePrefab, position.SetY(2f), Quaternion.identity);

    }
}

public interface ISquishAnim
{
    void OnSquish(Vector3 position);
}
