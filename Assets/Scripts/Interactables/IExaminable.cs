using System;
using System.Collections;

namespace Assets.Scripts.Interactables
{
    public interface IExaminable
    {
        IEnumerable Examine(Action callback);
    }
}