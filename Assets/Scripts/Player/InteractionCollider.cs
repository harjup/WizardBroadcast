using Assets.Scripts.Interactables;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class InteractionCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GetComponentInParent<UserInteract>().OnInteractionTriggerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        GetComponentInParent<UserInteract>().OnInteractionTriggerExit(other);
    }
}
