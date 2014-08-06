using UnityEngine;
using System.Collections;

public class GhostPlayer : MonoBehaviourBase
{
    private GameObject _fartParticle;
    void Start()
    {
        _fartParticle = Resources.Load("Particles/FartParticle") as GameObject;
    }

    public enum GhostAnim
    {
        Undefined,
        Fart
    }

    public string Id;
    public void Initialize(string _id, Vector3 position)
    {
        DontDestroyOnLoad(gameObject);

        Id = _id;
        transform.position = position;
        transform.GetChild(0).GetChild(0).renderer.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), .6f);
    }

    public void UpdatePosition(Vector3 position)
    {
        if (gameObject == null) return;
        if (_animating) return;

        iTween.Stop(gameObject);
        iTween.MoveTo(gameObject, iTween.Hash("position", position, "orienttopath", true, "axis", "y", "time", .3f, "easeType", iTween.EaseType.linear));
    }

    private bool _animating = false;
    public IEnumerator DoAnimation(GhostAnim anim)
    {
        if (anim == GhostAnim.Fart)
        {
            _animating = true;
            var particle = Instantiate(_fartParticle, transform.position, transform.rotation) as GameObject;
            iTween.MoveTo(gameObject, iTween.Hash("y", transform.position.y + 1f, "orienttopath", false, "time", 1f, "easeType", iTween.EaseType.easeOutElastic));
            //spawn a little fart cloud particle
            yield return new WaitForSeconds(1.25f);
            Destroy(particle);
            _animating = false;
        }
    }

    public void Remove()
    {
        Destroy(this.gameObject);
    }


}
