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
        if (IsSquished) return;
        var volume = other.GetComponent<DeathVolume>();
        if (volume != null)
        {
            Debug.Log(other.name);
            _isSquished = true;
            GetComponentInParent<PuzzleRoomManager>().UpdateRoomCompletion();

            //Ugh, really need to add a getinterface in children override
            var anim = GetComponentInChildren<SquishAnim>() as ISquishAnim;
            if (anim == null) anim = GetComponentInChildren<SquishAnimIce>() as ISquishAnim;

            anim.OnSquish(volume.transform.position);
        }
    }
}