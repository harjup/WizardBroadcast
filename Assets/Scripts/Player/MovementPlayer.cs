using UnityEngine;
using System.Collections;

namespace WizardBroadcast
{
    public class MovementPlayer : MonoBehaviourBase
    {

        private Vector3 _currentSpeed = new Vector3();
        private Rigidbody rigidbody;
        private float speed = 20;

        // Use this for initialization
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            _currentSpeed = _currentSpeed
                .SetX(Input.GetAxis("Horizontal") * speed)
                .SetZ(Input.GetAxis("Vertical")*speed);

            rigidbody.velocity = _currentSpeed;
        }
    }
}