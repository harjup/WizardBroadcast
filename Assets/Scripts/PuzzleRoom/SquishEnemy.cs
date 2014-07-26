using System.Collections;
using UnityEngine;

public class SquishEnemy : MonoBehaviourBase
{
    private bool _isSquished = false;
    public bool IsSquished
    {
        get { return _isSquished; }
    }

    public IEnumerator Die()
    {
        Destroy(gameObject);
        yield break;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DeathVolume>() != null)
        {
            _isSquished = true;
            GetComponentInParent<PuzzleRoomManager>().UpdateRoomCompletion();
            GetComponentInChildren<SquishAnim>().OnSquish();
        }
    }
}