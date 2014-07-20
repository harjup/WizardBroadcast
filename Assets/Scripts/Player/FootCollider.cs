using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WizardBroadcast;

public class FootCollider : MonoBehaviour
{

    public List<GameObject> OtherGameObjects = new List<GameObject>();

    void Start()
    {
        UpdateFloorState();
    }

    void OnLevelWasLoaded(int level)
    {
        OtherGameObjects.Clear();
        UpdateFloorState();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UserMovement>()) return;

        OtherGameObjects.Add(other.gameObject);
        UpdateFloorState();
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<UserMovement>()) return;

        OtherGameObjects.Remove(other.gameObject);
        UpdateFloorState();
    }

    void UpdateFloorState()
    {
        var nonNullObjects = new List<GameObject>();
        foreach (var otherGameObject in OtherGameObjects)
        {
            if (otherGameObject != null)
            {
                nonNullObjects.Add(otherGameObject);
            }
        }
        OtherGameObjects = nonNullObjects;

        GetComponentInParent<UserMovement>().AirState = OtherGameObjects.Count == 0;
    }
}
