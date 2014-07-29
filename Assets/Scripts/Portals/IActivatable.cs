using System;

namespace Assets.Scripts.Portals
{
    public interface IActivatable
    {
        void Activate(Action callback);
    }
}