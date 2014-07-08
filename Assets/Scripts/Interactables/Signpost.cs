using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class Signpost : MonoBehaviourBase
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InfoPlayer>() != null)
        {
            
        }

    }
}
