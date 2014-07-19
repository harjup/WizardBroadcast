﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameState;
using Assets.Scripts.Interactables;
using Assets.Scripts.Managers;
using Assets.Scripts.Portals;
using TreeEditor;
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
        private PlayerAnimate _animator;
        //TODO: Put initial level position stuff in its own thing
        //TODO: Put ghost stuff in own thing
        

        void Start()
        {
            _animator = GetComponent<PlayerAnimate>();
            if (_animator == null)
            {
                Debug.LogError("Expected PlayerAnimate component on player, was not found");
            }

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
            transform.position = CheckpointStore.Instance.ActiveSpawnMarker.transform.position;
        }

        void SendGhostInfo()
        {
            SignalrEndpoint.Instance.SendPositionUpdate(transform.position);
        }

        public void OnTreasureCollected(TreasureType type)
        {
            StartCoroutine(_animator.DoCollectedThingDance(type));
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
            rigidbody.useGravity = false;
            yield return StartCoroutine(_animator.VerticalHoleTransition(targetEndpoint, enterMethod));
            rigidbody.useGravity = true;
            InputManager.Instance.PlayerInputEnabled = true;
            gameObject.collider.enabled = true;
            //Move player to initial position
            //Start iTweening toward target
            //Togggle correct player animation
            //CameraFadeOut
            //Move player to target position
        }
    }
}
