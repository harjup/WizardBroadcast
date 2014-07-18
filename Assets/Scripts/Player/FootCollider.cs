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
        GetComponentInParent<UserMovement>().SetAirState(OtherGameObjects.Count == 0);
    }
}
