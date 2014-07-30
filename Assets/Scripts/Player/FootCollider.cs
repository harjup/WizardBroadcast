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
        //UpdateFloorState();
        //StartCoroutine(UpdateFloorTouch());
    }

    public bool inAir = true;
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.lossyScale.x);
        inAir = true;

        foreach (var other in colliders)
        {
            if (other.isTrigger) continue;
            if (other.GetComponent<UserMovement>()) continue;
            if (other.GetComponent<RoomEnterTrigger>()) continue;
            if (other.GetComponentInParent<SpawnMarker>()) continue;
            if (other.GetComponentInParent<FogTrigger>()) continue;
            if (other.GetComponentInParent<PushPoint>()) continue;
            inAir = false;
        }

        GetComponentInParent<UserMovement>().AirState = inAir;

    }

/*
    
    IEnumerator UpdateFloorTouch()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            GetComponentInParent<UserMovement>().AirState = inAir;
            inAir = true;    
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<UserMovement>()) return;
        if (other.GetComponent<RoomEnterTrigger>()) return;
        if (other.GetComponentInParent<SpawnMarker>()) return;
        if (other.GetComponentInParent<FogTrigger>()) return;
        if (other.GetComponentInParent<PushPoint>()) return;

        inAir = false;
        //UpdateFloorState();
    }
*/

/*

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
*/




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
