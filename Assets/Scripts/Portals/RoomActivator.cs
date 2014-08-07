using System;
using System.Linq;
using Assets.Scripts.Portals;
using UnityEngine;
using System.Collections;

public class RoomActivator : MonoBehaviourBase, IActivatable
{
    int _callbacks = 0;
    public void Activate(Action callback)
    {
        int finishedCallbacks = 0;
        var triggers = GetComponentsInChildren(typeof (IActivatable)).Cast<IActivatable>();
        foreach (var activatable in triggers)
        {
            //Somehow we are grabbing ourself here
            if (activatable.Equals(this)) continue;

            _callbacks++;
            activatable.Activate(() =>
            {
                finishedCallbacks++;
                if (finishedCallbacks == _callbacks)
                {
                    callback();
                    Destroy(gameObject);
                }
            });
        }
    }
}
