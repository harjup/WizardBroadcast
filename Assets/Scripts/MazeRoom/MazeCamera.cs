using UnityEngine;
using System.Collections;

public class MazeCamera : MonoBehaviour
{
    public bool Enabled
    {
        set { camera.enabled = value; }
    }
}
