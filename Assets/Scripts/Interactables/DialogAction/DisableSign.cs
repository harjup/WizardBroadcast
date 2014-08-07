using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interactables.DialogAction
{
    class DisableSign : TextAction
    {
        public override void Execute()
        {
            //I could figure out how to integrate this with how hte user picks up interactables.
            //Or I could just move it, I guess :effort:
            GetComponentInParent<Signpost>().transform.position = new Vector3(0f,-100f,0f);
        }
    }
}
