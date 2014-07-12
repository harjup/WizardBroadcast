using System;
using System.Collections;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Interactables
{
    public enum TreasureType
    {
        Undefined,
        Little,
        Medium,
        Large
    }

    public class Treasure : MonoBehaviourBase
    {
        public TreasureType Type;
        
        private int _pointCount;
        private Color _myColor;

        void Start () 
        {
            //TODO: Swap this out with a constucting a proper type and use that to determine properties
            switch (Type)
            {
                case TreasureType.Undefined:
                    Debug.LogError("Tried to use a collectable with an unset Type");
                    break;
                case TreasureType.Little:
                    _pointCount = 1;
                    _myColor = Color.blue;
                    break;
                case TreasureType.Medium:
                    _pointCount = 5;
                    _myColor = Color.red;
                    break;
                case TreasureType.Large:
                    _pointCount = 10;
                    _myColor = Color.yellow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //Add a little rotation animation
            iTween.RotateAdd(gameObject, iTween.Hash("y", 360, "looptype" ,iTween.LoopType.pingPong, "easetype", iTween.EaseType.spring, "speed", 200f));

            renderer.material.color = _myColor;
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            var playerComponent = other.GetComponent<InfoPlayer>();
            if (playerComponent != null)
            {
                playerComponent.OnTreasureCollected(Type);
                StartCoroutine(Despawn());
            }
        }

        IEnumerator Despawn()
        {
            //Play animation
            //Make a particle effect
            //When it's done kill the object
            Destroy(this.gameObject);
            yield return null;
        }

    }
}
