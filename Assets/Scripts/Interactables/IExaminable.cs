using System;
using System.Collections;

namespace Assets.Scripts.Interactables
{
    public interface IExaminable
    {
        IEnumerator Examine(Action callback);
    }
}