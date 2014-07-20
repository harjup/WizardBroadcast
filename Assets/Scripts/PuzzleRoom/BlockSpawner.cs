using UnityEngine;
using System.Collections;

public class BlockSpawner : MonoBehaviour
{
    public bool IsSlippery = false;

    void Start()
    {
        renderer.enabled = false;
    }
}
