using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameState;
using Assets.Scripts.Interactables;
using Assets.Scripts.Managers;
using Assets.Scripts.Portals;
using UnityEngine;
using WizardBroadcast;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Currently using this class to identify that a given object is the player.
    /// It can be the entrypoint for any messages from outside components
    /// There shouldn't be any logic in here other than passing things off to the components that handle them
    /// </summary>
    class InfoPlayer : MonoBehaviourBase
    {

        //TODO: Put initial level position stuff in its own thing
        //TODO: Put ghost stuff in own thing
        void Start()
        {
            GhostPositionUpdate();
            MoveToStartPosition();
        }

        void OnLevelWasLoaded(int level)
        {
            GhostPositionUpdate();
            MoveToStartPosition();
        }

        void GhostPositionUpdate()
        {
            if (Application.loadedLevelName == SceneMap.GetScene(Scene.Hub))
            {
                SignalrEndpoint.Instance.StartGhost();
                InvokeRepeating("SendGhostInfo", 1f, 1f);
            }
            else
            {
                CancelInvoke("SendGhostInfo");
            }
        }

        void MoveToStartPosition()
        {
            //TODO: Be move selective on when spawnMarker to user based on application state and whatever
            var spawnMarker = FindObjectsOfInterface<SpawnMarker>();
            if (spawnMarker.Count > 0)
            {
                transform.position = spawnMarker[0].transform.position;
            }
        }

        void SendGhostInfo()
        {
            SignalrEndpoint.Instance.SendPositionUpdate(transform.position);
        }

        public void OnTreasureCollected(TreasureType type)
        {
            StartCoroutine(DoCollectedThingDance(type));
            
        }


        public enum TransitionType
        {
            Undefined,
            FallDown,
            WalkForward
        }

        //Let's start specific and get generic as we go ok
        public IEnumerator OnFellDownHole(Vector3 targetEndpoint, GrottoEntrance.EnterMethod enterMethod)
        {
            InputManager.Instance.PlayerInputEnabled = false;
            gameObject.collider.enabled = false;
             yield return StartCoroutine(VerticalHoleTransition(targetEndpoint, enterMethod));
            InputManager.Instance.PlayerInputEnabled = true;
            gameObject.collider.enabled = true;
            //Move player to initial position
            //Start iTweening toward target
            //Togggle correct player animation
            //CameraFadeOut
            //Move player to target position
        }

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

        //TODO: Give this guy a better home
        public IEnumerator DoCollectedThingDance(TreasureType type)
        {
            InputManager.Instance.PlayerInputEnabled = false;
            iTween.MoveTo(gameObject, transform.position.SetY(transform.position.y + 1), .5f);
            yield return new WaitForSeconds(1f);
            iTween.MoveTo(gameObject, transform.position.SetY(transform.position.y - 1), .5f);
            yield return new WaitForSeconds(.5f);
            InputManager.Instance.PlayerInputEnabled = true;
            UserProgressStore.Instance.AddTreasure(type);
        }  
    }
}
