using UnityEngine;

namespace Assets.Scripts.Interactables
{
    //Now if you want to know if your child triggers ran into something, you can 
    //just find all children that implement ITrigger
    public class Trigger : MonoBehaviour, ITrigger
    {
        public delegate void TriggerEnterHandler(Collider other);

        private event TriggerEnterHandler TriggerEnterEvent;

        protected virtual void OnTriggerEnterEvent(Collider other)
        {
            TriggerEnterHandler handler = TriggerEnterEvent;
            if (handler != null) handler(other);    
        }


        //OnTriggerEnter, alert all subscribers that you ran into something
        void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent(other);
        }

        public void Subscribe(TriggerEnterHandler del)
        {
            TriggerEnterEvent += del;
        }

        public void Unsubscribe(TriggerEnterHandler del)
        {
            TriggerEnterEvent -= del;
        }
    }
}
