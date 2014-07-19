using System;
using System.Collections;
using System.Globalization;
using Assets.Scripts.Interactables;
using Assets.Scripts.Managers;
using Assets.Scripts.Portals;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using WizardBroadcast;

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
            yield return new WaitForSeconds(.25f);
            yield return StartCoroutine(CameraManager.Instance.DoWipeOut(.5f));
            yield return new WaitForSeconds(.5f);
            CameraManager.Instance.GetCameraRig().position = targetEndpoint;
            transform.position = targetEndpoint.SetY(targetEndpoint.y + 3f);
            iTween.MoveTo(gameObject, iTween.Hash("position", targetEndpoint, "time", 1f, "easetype", iTween.EaseType.easeOutCirc));
            yield return new WaitForSeconds(.25f);
            yield return StartCoroutine(CameraManager.Instance.DoWipeIn(.5f));
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


        private bool _isMessedUpRunning = false;
        public IEnumerator GetMessedUp()
        {
            if (_isMessedUpRunning) yield break;
            _isMessedUpRunning = true;

            InputManager.Instance.PlayerInputEnabled = false;
            iTween.RotateTo(gameObject, iTween.Hash("rotation", new Vector3(90f, 0f, 0f), "time", 1f, "easetype", iTween.EaseType.easeOutBounce));
            yield return new WaitForSeconds(.5f);
            yield return StartCoroutine(CameraManager.Instance.DoWipeOut(1f));
            yield return new WaitForSeconds(.5f);
            transform.position = CheckpointStore.Instance.ActiveSpawnMarker.transform.position;
            transform.rotation = Quaternion.identity;
            yield return StartCoroutine(CameraManager.Instance.DoWipeIn(.5f));
            InputManager.Instance.PlayerInputEnabled = true;
            _isMessedUpRunning = false;
        }
    }
}
