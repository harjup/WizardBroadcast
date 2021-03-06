﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Player;

using UnityEngine;

namespace Assets.Scripts.Portals
{
    public class GrottoEntrance : MonoBehaviourBase
    {
        public Transform targetEndpoint;

        public enum EnterMethod
        {
            Undefined,
            Fall,
            Spring,
            WalkForward
        }

        public EnterMethod Enter;

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
                if (Enter == EnterMethod.WalkForward)
                {
                    StartCoroutine(playerInfo.OnEnterDoorway(Vector3.forward, targetEndpoint));
                    return;
                }
                
                StartCoroutine(playerInfo.OnFellDownHole(targetEndpoint.position, Enter));
            }
        }
    }
}
