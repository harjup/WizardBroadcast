using System;
using System.Collections;
using System.Globalization;
using Assets.Scripts.Interactables;
using Assets.Scripts.Managers;
using Assets.Scripts.Portals;
using UnityEngine;

namespace Assets.Scripts.Player
{
    class PlayerAnimate : MonoBehaviourBase
    {

        public void OnLevelTransitionOut()
        {
            //Disable player input
            //Move player to initial position
            //Start iTweening toward target
            //Togggle correct player animation
            //CameraFadeOut
            //Move player to target position
        }

        public void OnLevelTransitionIn()
        {
            //Move player to initial position
            //Togggle correct player animation
            //Disable player input
            //CameraFadeIn
            //Start iTweening toward target
            //Move player to target position
            //Enable input
        }


        public IEnumerator VerticalHoleTransition(Vector3 targetEndpoint, GrottoEntrance.EnterMethod enterMethod)
        {
            if (enterMethod == GrottoEntrance.EnterMethod.Fall)
            {
                iTween.MoveTo(gameObject, iTween.Hash("position", transform.position.SetY(transform.position.y - 5f), "time", .5f, "easetype", iTween.EaseType.easeInBack));
            }
            else if (enterMethod == GrottoEntrance.EnterMethod.Spring)
            {
                iTween.MoveTo(gameObject, iTween.Hash("position", transform.position.SetY(transform.position.y + 10f), "time", .5f, "easetype", iTween.EaseType.easeInOutSine));
            }
            yield return new WaitForSeconds(.5f);
            //TODO: Fade out camera
            yield return new WaitForSeconds(.5f);
            //TODO: Fade in camera
            yield return new WaitForSeconds(.5f);
            transform.position = targetEndpoint.SetY(targetEndpoint.y + 3f);
            Camera.main.transform.parent.position = targetEndpoint;
            iTween.MoveTo(gameObject, iTween.Hash("position", targetEndpoint, "time", 1f, "easetype", iTween.EaseType.easeOutCirc));
            yield return new WaitForSeconds(1f);
        }

        public IEnumerator DoCollectedThingDance(TreasureType type)
        {
            rigidbody.useGravity = false;
            rigidbody.velocity = rigidbody.velocity.SetY(0f);
            if (type == TreasureType.Little)
            {
                yield return StartCoroutine(LittleTreasureDance());
            }
            else
            {
                InputManager.Instance.PlayerInputEnabled = false;
                iTween.MoveTo(gameObject, transform.position.SetY(transform.position.y + 1), .5f);
                yield return new WaitForSeconds(.5f);
                iTween.MoveTo(gameObject, transform.position.SetY(transform.position.y - 1), .5f);
                yield return new WaitForSeconds(.5f);
                InputManager.Instance.PlayerInputEnabled = true;
            }
            rigidbody.useGravity = true;
            UserProgressStore.Instance.AddTreasure(type);
        }

        public IEnumerator LittleTreasureDance()
        {
            //Just show it above their head I guess
            yield return new WaitForSeconds(.05f);
        }
    }
}
