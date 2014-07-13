using UnityEngine;
using System.Collections;

public class SpawnObject : TextAction
{

    public GameObject Prefab;
    public Transform Transform;

    public override void Execute()
    {
        Instantiate(Prefab, Transform.position, new Quaternion());
    }
}
