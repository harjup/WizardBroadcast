using UnityEngine;
using System.Collections;

public class GhostPlayer : MonoBehaviourBase
{

    private string id;

    void SetPosition(Vector3 positon)
    {
        transform.position = positon;
    }

    void UpdatePosition(Vector3 position)
    {
        iTween.Stop(gameObject);
        iTween.MoveTo(gameObject, iTween.Hash("position", position, "orienttopath", true, "axis", "y", "speed", 1, "easeType", iTween.EaseType.easeInOutCubic));
    }

}
