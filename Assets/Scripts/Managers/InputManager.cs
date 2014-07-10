
using System;
using System.Collections;
using UnityEngine;

//This will need to be first in script execution order so other scripts can get inputs on the same frame
namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Abstracts inputs behind a singleton object and 
    /// allows other scripts to enable or disable different input groups, 
    /// which cause those inputs to always return as their default value
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        private float _horitzontalAxis;
        private float _verticalAxis;
        private float _rawVerticalAxis;
        private float _rawHoritzontalAxis;
        private bool _cameraAction;
        private bool _interactAction;

        private bool _playerInputEnabled = true;
        private bool _cameraInputEnabled = true;
        private bool _playerMovementEnabled = true;


        public float HoritzontalAxis
        {
            get { return _playerMovementEnabled && _playerInputEnabled ? _horitzontalAxis : 0; }
           private set { _horitzontalAxis = value; }
        }

        public float VerticalAxis
        {
            get
            {
                return _playerMovementEnabled && _playerInputEnabled ? _verticalAxis : 0;
            }
            private set { _verticalAxis = value; }
        }

        public float RawHoritzontalAxis
        {
            get { return _playerMovementEnabled && _playerInputEnabled ? _rawHoritzontalAxis : 0; }
            private set { _rawHoritzontalAxis = value; }
        }

        public float RawVerticalAxis
        {
            get
            {
                return _playerMovementEnabled && _playerInputEnabled ? _rawVerticalAxis : 0;
            }
            private set { _rawVerticalAxis = value; }
        }

        public bool CameraAction
        {
            get
            {
                return _cameraAction
                    && _playerInputEnabled 
                    && _cameraInputEnabled;
            }
            private set { _cameraAction = value; }
        }

        public bool InteractAction
        {
            get
            {
                return _interactAction 
                    && _playerInputEnabled;
            }
            private set { _interactAction = value; }
        }


        void Update()
        {
            //Get all the inputs for da frame
            VerticalAxis = Input.GetAxis("Vertical");
            RawVerticalAxis = Input.GetAxisRaw("Vertical");

            HoritzontalAxis = Input.GetAxis("Horizontal");
            RawHoritzontalAxis = Input.GetAxisRaw("Horizontal");
            
            InteractAction = Input.GetKeyDown(KeyCode.Z);
            CameraAction = Input.GetKeyDown(KeyCode.X);
        }
        
        public bool PlayerInputEnabled
        {
            private get { return _playerInputEnabled; }
            set{ _playerInputEnabled = value;}
        }

        public bool PlayerMovementEnabled
        {
            private get { return _playerMovementEnabled; }
            set { _playerMovementEnabled = value; }
        }

        public bool CameraControlEnabled
        {
            private get { return _cameraInputEnabled; }
            set { _cameraInputEnabled = value; }
        }
    }
}

