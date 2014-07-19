using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class SpawnMarker : MonoBehaviour
{
    public bool firstSpawnMarker;

    void Awake()
    {
        if (!firstSpawnMarker) return;
        CheckpointStore.Instance.ActiveSpawnMarker = this;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InfoPlayer>() != null)
        {
            CheckpointStore.Instance.ActiveSpawnMarker = this;
        }
    }
}
