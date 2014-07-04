using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Player
{
    class InfoPlayer : MonoBehaviourBase
    {
        //TODO: Look for a spawner and set position via that
        void OnLevelWasLoaded(int level)
        {
            transform.position = new Vector3(0f,2.5f,0f);
        }
        
        void Start()
        {
            
        }
    }
}
