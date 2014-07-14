using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Portals;
using UnityEngine;

class FakeDoor : MonoBehaviourBase, IActivatable
{
    public void Activate()
    {
        gameObject.SetActive(false);
    }
}

