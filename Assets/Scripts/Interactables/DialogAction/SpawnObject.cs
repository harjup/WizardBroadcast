using UnityEngine;
using System.Collections;

public class SpawnObject : TextAction
{

    public GameObject Prefab;
    public Transform Transform;
    public float delayInSeconds = 0f;

    public override void Execute()
    {
        StartCoroutine(SpawnAfterSeconds());
    }

    IEnumerator SpawnAfterSeconds()
    {
        yield return new WaitForSeconds(delayInSeconds);
        Instantiate(Prefab, Transform.position, new Quaternion());
    }
}
