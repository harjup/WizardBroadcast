using UnityEngine;

public class EnemySpawner : MonoBehaviourBase
{
    public bool isIce = false;

    void Start()
    {
        renderer.enabled = false;
    }
}