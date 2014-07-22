using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class CameraLookAt : MonoBehaviour
{
    private Transform playerTransform;
    void Start()
    {
        playerTransform = FindObjectOfType<InfoPlayer>().transform;
    }

    void Update()
    {
        iTween.LookUpdate(gameObject, iTween.Hash("looktarget", playerTransform.position, "axis", "y", "time", 1f));
    }
}
