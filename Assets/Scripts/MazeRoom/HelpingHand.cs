using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class HelpingHand : MonoBehaviour
{
    private Transform playerTransform;
    private bool _chasingPlayer = true;

    void Start()
    {
        playerTransform = FindObjectOfType<InfoPlayer>().transform;
        transform.position = transform.position.SetY(playerTransform.position.y + 20f);
    }

    void Update()
    {
        if (!_chasingPlayer) return;

        transform.LookAt(playerTransform);
        transform.position = transform.position.SetY(iTween.FloatUpdate(transform.position.y, playerTransform.position.y, 1f));

        //TODO: Have the camera contextually recenter or some shit
        var positionDifference = playerTransform.position - transform.position;
        float xSpeed = Mathf.Abs(positionDifference.x) * 5f;
        float zSpeed = Mathf.Abs(positionDifference.z) * 5f;

        //Cap the camera's speed so it doesn't go fucking nuts and start overshooting the player
        if (xSpeed > 40) { xSpeed = 40; }
        if (zSpeed > 40) { zSpeed = 40; }

        if (Mathf.Abs(positionDifference.x) >= .5f)
        {
            transform.position = transform.position.SetX(iTween.FloatUpdate(transform.position.x, playerTransform.position.x, xSpeed));
        }
        if (Mathf.Abs(positionDifference.z) >= .5f)
        {
            transform.position = transform.position.SetZ(iTween.FloatUpdate(transform.position.z, playerTransform.position.z, zSpeed));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InfoPlayer>() != null)
        {
            _chasingPlayer = false;
            iTween.ShakePosition(gameObject, iTween.Hash("amount", Vector3.forward, "time", .5f, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.easeInOutExpo));
            Destroy(gameObject, 2f);
        }
    }
}
