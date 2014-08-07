using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Portals
{
    class FallDownWall : MonoBehaviourBase, IActivatable
    {
        public void Activate(Action callback)
        {
            StartCoroutine(Animate(callback));
        }

        IEnumerator Animate(Action callback)
        {
            iTween.RotateTo(gameObject, iTween.Hash("x", 90f, "time", 1f, "easetype", iTween.EaseType.easeOutBounce));
            yield return new WaitForSeconds(1.5f);
            callback();
        } 
    }
}
