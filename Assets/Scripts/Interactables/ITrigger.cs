using UnityEngine;

namespace Assets.Scripts.Interactables
{
    public interface ITrigger
    {
        void Subscribe(Trigger.TriggerEnterHandler del);
        void Unsubscribe(Trigger.TriggerEnterHandler del);
    }
}
