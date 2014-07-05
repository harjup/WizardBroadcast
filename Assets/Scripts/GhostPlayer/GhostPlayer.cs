using UnityEngine;
using System.Collections;

public class GhostPlayer : MonoBehaviourBase
{

    public string id;

    void Initialize(string _id, Vector3 position)
    {
        id = _id;
        transform.position = position;
    }

    void UpdatePosition(Vector3 position)
    {
        iTween.Stop(gameObject);
        iTween.MoveTo(gameObject, iTween.Hash("position", position, "orienttopath", true, "axis", "y", "speed", 1, "easeType", iTween.EaseType.easeInOutCubic));
    }

}
