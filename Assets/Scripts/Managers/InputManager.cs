/*
using System;
using System.Collections;
using UnityEngine;

//This will need to be first in script execution order so other scripts can get inputs on the same frame
namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Listens for all inputs and maps them to intents
    /// Intents can be enabled and disabled
    /// </summary>
    class InputManager : Singleton<InputManager>
    {
      //Player forward
      //Player left/right
      //Camera adjust  
        
    
        void Update(){
           //Capture horizontal and vertical axis and map them to userInput
           //player movement needs to express up, down, left, right, or a combination.
           
           //Each button can have custom logic for whether it's being held down or not because Input.getKeyDown sucks
           
           
        }
        
        void SetPlayerInputEnabled(bool value){
        
        }
        
        void SetPlayerMovementEnabled(bool value){
        
        }
        
        void SetCameraControlEnabled(bool value){
        
        }
        
    }
    
    /*
    enum buttonState{
      Undefined,
      Up,
      Down,
      Held
    }
    */
    
    /* //Maybe something like this?
    //Then I can jsut check if a thing is true and proceed accordingly
    struct UserInput {
        bool forward;
        bool back;
        bool left;
        bool right;
        bool centerCamera;
        bool actionA;
        bool actionB;
    }
    */
}
*/
