using UnityEngine;
using System.Collections;

public class GhostPlayer : MonoBehaviourBase
{

    public string id;

    public void Initialize(string _id, Vector3 position)
    {
        id = _id;
        transform.position = position;
    }

    public void UpdatePosition(Vector3 position)
    {
        iTween.Stop(gameObject);
        iTween.MoveTo(gameObject, iTween.Hash("position", position, "orienttopath", true, "axis", "y", "time", 1f, "easeType", iTween.EaseType.linear));
    }

    public void Remove()
    {
        Destroy(this.gameObject);
    }

}
