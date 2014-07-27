using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class PuzzleResetButton : MonoBehaviour
{
    private bool _resettingRoom = false;
    void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<InfoPlayer>() != null && !_resettingRoom)
        {
            Debug.Log("Resetting Room!!");
            StartCoroutine(ResetRoom());
        }
    }

    IEnumerator ResetRoom()
    {
        _resettingRoom = true;
        yield return StartCoroutine(GetComponentInParent<PuzzleRoomManager>().RoomInit());
        _resettingRoom = false;
    }
}
