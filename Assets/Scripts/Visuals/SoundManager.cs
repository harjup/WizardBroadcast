using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//May or may not actually use this
public class SoundManager : Singleton<SoundManager>
{
    public enum Effect
    {
        Undefined,
        Walk
    }

    private AudioSource _source;

    public void Play(Effect effect, bool isLooping)
    {
        
    }

    public void Stop(Effect effect)
    {

    }

    public void StopAll()
    {

    }
}
