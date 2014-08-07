﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Interactables
{
    /// <summary>
    /// Pretty please don't write concrete methods in here, pretend it's an abstract class ok
    /// 
    /// Let'ss try converting this into an interface maybe?? 
    /// _interactable = (Interactable)target.GetComponent(typeof(Interactable))
    /// </summary>
    public class ExaminableBase : MonoBehaviourBase
    {
        public virtual IEnumerator Examine(Action callback)
        {
            yield return null;
        }
    }
}
