using System.Collections;
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
            if (Application.loadedLevelName != SceneMap.GetScene(Scene.Start))
            {
                GhostPositionUpdate();
                MoveToStartPosition();
            }
        }

        void GhostPositionUpdate()
        {
            if (Application.loadedLevelName == SceneMap.GetScene(Scene.Hub))
            {
                SignalrEndpoint.Instance.StartGhost();
                InvokeRepeating("SendGhostInfo", 1f, .25f);
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
            if (enterMethod == GrottoEntrance.EnterMethod.Spring) SoundManager.Instance.Play(SoundManager.SoundEffect.Teleport);

            InputManager.Instance.PlayerInputEnabled = false;
            gameObject.collider.enabled = false;
            rigidbody.useGravity = false;
            yield return StartCoroutine(_animator.VerticalHoleTransition(targetEndpoint, enterMethod));
            rigidbody.useGravity = true;
            InputManager.Instance.PlayerInputEnabled = true;
            gameObject.collider.enabled = true;

            if (enterMethod == GrottoEntrance.EnterMethod.Fall) SoundManager.Instance.Play(SoundManager.SoundEffect.MainLand);

            //Move player to initial position
            //Start iTweening toward target
            //Togggle correct player animation
            //CameraFadeOut
            //Move player to target position
        }

        public IEnumerator OnEnterDoorway(Vector3 targetDirection, Transform destination, RoomManager newRoom = null, RoomManager oldRoom = null)
        {
            InputManager.Instance.PlayerInputEnabled = false;
            gameObject.collider.enabled = false;
            rigidbody.useGravity = false;
            yield return StartCoroutine(_animator.WalkForwardTransition(targetDirection, destination, newRoom, oldRoom));
            rigidbody.useGravity = true;
            InputManager.Instance.PlayerInputEnabled = true;
            gameObject.collider.enabled = true;
        }
    }
}
