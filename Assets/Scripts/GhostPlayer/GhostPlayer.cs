using UnityEngine;
using System.Collections;

public class GhostPlayer : MonoBehaviourBase
{

    public string id;

    public void Initialize(string _id, Vector3 position)
    {
        id = _id;
        transform.position = position;
        transform.GetChild(0).GetChild(0).renderer.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), .6f);

    }

    public void UpdatePosition(Vector3 position)
    {
        if (gameObject == null) return;

        iTween.Stop(gameObject);
        iTween.MoveTo(gameObject, iTween.Hash("position", position, "orienttopath", true, "axis", "y", "time", 1f, "easeType", iTween.EaseType.linear));
    }

    public void Remove()
    {
        Destroy(this.gameObject);
    }

}
