using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class SpawnMarker : MonoBehaviour
{
    public bool firstSpawnMarker;

    //Runs between Awake (Checkpoint store created) and Start (InfoPlayer finds active spawnmarker)
    void Awake()
    {
        if (!firstSpawnMarker) return;

        if (CheckpointStore.Instance == null)
        {
            Instantiate(Resources.Load(@"Prefabs/CheckpointStore", typeof(GameObject)), Vector3.zero, new Quaternion());
        }

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
