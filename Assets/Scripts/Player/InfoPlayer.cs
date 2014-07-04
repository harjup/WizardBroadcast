﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Player
{
    class InfoPlayer : MonoBehaviourBase
    {
        //TODO: Determine if this is the best spot to put start position logic stuff
        void Start()
        {
            MoveToStartPosition();
        }
        void OnLevelWasLoaded(int level)
        {
            MoveToStartPosition();
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
    }
}
