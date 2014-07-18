using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockTop : MonoBehaviour
{
    //private bool _isBlocked = false;

    public List<GameObject> OtherGameObjects = new List<GameObject>(); 

    public bool IsBlocked
    {
        get
        {
            return OtherGameObjects.Count != 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractionCollider>() != null) return;

        OtherGameObjects.Add(other.gameObject);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractionCollider>() != null) return;

        OtherGameObjects.Remove(other.gameObject);
    }
}
