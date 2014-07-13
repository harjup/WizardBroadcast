using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Player;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace Assets.Scripts.Portals
{
    public class GrottoEntrance : MonoBehaviourBase, IActivatable
    {
        public Transform targetEndpoint;

        public enum EnterMethod
        {
            Undefined,
            Fall,
            Spring
        }

        public EnterMethod Enter;

        public void Activate()
        {
            //TODO: Wire this part up better
            transform.FindChild("Mesh").gameObject.SetActive(true);
        }

        void OnTriggerEnter(Collider other)
        {
            if (Enter == EnterMethod.Undefined)
            {
                Debug.LogError("Tried to enter a grotto with an undefined entrance type");
                return;
            }

            var playerInfo = other.GetComponent<InfoPlayer>();
            if (other.GetComponent<InfoPlayer>() != null)
            {
                StartCoroutine(playerInfo.OnFellDownHole(targetEndpoint.position, Enter));
            }
        }
    }
}
