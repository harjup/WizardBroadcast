using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interactables;
using Assets.Scripts.Managers;
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

        public void OnTreasureCollected(Collectable.TreasureType type)
        {
            StartCoroutine(DoCollectedThingDance());
        }

        //TODO: Give this guy a better home
        public IEnumerator DoCollectedThingDance()
        {
            InputManager.Instance.PlayerInputEnabled = false;
            iTween.MoveTo(gameObject, transform.position.SetY(transform.position.y + 1), .5f);
            yield return new WaitForSeconds(1f);
            iTween.MoveTo(gameObject, transform.position.SetY(transform.position.y - 1), .5f);
            yield return new WaitForSeconds(.5f);
            InputManager.Instance.PlayerInputEnabled = true;
        }  
    }
}
