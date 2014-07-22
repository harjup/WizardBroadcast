using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class RoomEnterTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InfoPlayer>())
        {
            GetComponentInParent<MazeRoomManager>().OnRoomEnter();
        }
    }
}
