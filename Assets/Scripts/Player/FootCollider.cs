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
        //TODO This is terrible I am sorry
        if (other.GetComponent<UserMovement>()) return;
        if (other.GetComponent<RoomEnterTrigger>()) return;
        if (other.GetComponentInParent<SpawnMarker>()) return;
        if (other.GetComponentInParent<FogTrigger>()) return;

        OtherGameObjects.Add(other.gameObject);
        UpdateFloorState();
    }
    void OnTriggerExit(Collider other)
    {
        //This is x2 terrible I am sorry
        if (other.GetComponent<UserMovement>()) return;
        if (other.GetComponent<RoomEnterTrigger>()) return;
        if (other.GetComponentInParent<SpawnMarker>()) return;
        if (other.GetComponentInParent<FogTrigger>()) return;

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
