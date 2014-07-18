using UnityEngine;
using System.Collections;

public class ColliderDebug : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + "hit a" + other.gameObject.name + "!");
    }
}
