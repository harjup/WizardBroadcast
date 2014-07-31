using UnityEngine;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{

    //Ugh. I'm sorry.
    //These enums shoudl be split into different categories so they're more managable with different dictionaries as well.
    //But.... effort...
    public enum SoundEffect
    {
        None,
        BlockDisengage,
        BlockMoveIce,
        BlockMoveStone1,
        BlockMoveStone2,
        BlockMoveStop,

        Squish1,
        Squish2,


        EffortNoise,
        HurtNoise,
        MainLand,

        FanFare,
        FanFare2,
        Teleport,
        TeleportAlt,
        Walk,

        BeepShort,
        BeepYes,
        BeepNo,
        BeepMaybe
    }

    
    public Dictionary<SoundEffect, Sound> SoundMap = new Dictionary<SoundEffect, Sound>()
    {
        //Blocksss
        {SoundEffect.BlockDisengage, new Sound("blockdisengage")},
        {SoundEffect.BlockMoveIce, new Sound("blockmove_ice")},
        {SoundEffect.BlockMoveStone1, new Sound("blockmove_stone1-2", .7f)},
        {SoundEffect.BlockMoveStone2, new Sound("blockmove_stone2-2")},
        {SoundEffect.BlockMoveStop, new Sound("blockmove_stop(new)")},
        
        //Squish Enemey
        {SoundEffect.Squish1, new Sound("squish1")},
        {SoundEffect.Squish2, new Sound("squish2")},

        //Fanfare
        {SoundEffect.FanFare, new Sound("Fanfare")},
        {SoundEffect.FanFare2, new Sound("Fanfare_2")},
        
        //Teleport
        {SoundEffect.Teleport, new Sound("teleport")},
        {SoundEffect.TeleportAlt, new Sound("teleport_alt", .75f, 3.2f)},

        //Walking will need to be its own thing
        {SoundEffect.Walk, new Sound("step_grass1")},

        //Player sfx
        {SoundEffect.EffortNoise, new Sound("effortnoise", .25f, .2f)},
        {SoundEffect.HurtNoise, new Sound("hurtnoise", .25f)},
        {SoundEffect.MainLand, new Sound("landsafe", .5f)},

        //UI blips and bloops
        {SoundEffect.BeepShort, new Sound("beepbeep")},
        {SoundEffect.BeepYes, new Sound("beepyes")},
        {SoundEffect.BeepNo, new Sound("beepno")},
        {SoundEffect.BeepMaybe, new Sound("beepmaybe")},
    };

    private List<AudioSource> _sources = new List<AudioSource>();

    void Awake()
    {
        foreach (var sound in SoundMap)
        {
            sound.Value.LoadClip();
        }
    }


    public void Play(SoundEffect effect, bool isLooping = false)
    {
        Debug.Log("Player sound " + effect);

        //First check if there are any sources that are not playing
        //If there is, throw the clip in that slot
        foreach (var audioSource in _sources)
        {
            Sound sound = SoundMap[effect];
            //Let's have it so there can only be one instance of any sound type, for ~now~
            if (audioSource.clip == sound.Clip)
            {
                PlayClip(audioSource, sound, isLooping);
                return;
            }

            if (audioSource.isPlaying == false)
            {
                audioSource.clip = SoundMap[effect].Clip;
                PlayClip(audioSource, sound, isLooping);
                audioSource.Play();
                return;
            }
        }
        //Otherwise add a new audiosource and throw the clip on that
        var newSource = gameObject.AddComponent<AudioSource>();
        PlayClip(newSource, SoundMap[effect], isLooping);
        _sources.Add(newSource);
    }

    private void PlayClip(AudioSource source, Sound sound, bool isLooping)
    {
        source.loop = isLooping;
        source.volume = sound.Volume;
        source.time = sound.StartTime;
        source.Play();
    }

    public bool IsPlaying(SoundEffect effect)
    {
        foreach (var audioSource in _sources)
        {
            //Let's have it so there can only be one instance of any sound type, for ~now~
            if (audioSource.clip == SoundMap[effect].Clip)
            {
                return audioSource.isPlaying;
            }
        }
        return false;
    }

    public void Stop(SoundEffect effect)
    {
        foreach (var audioSource in _sources)
        {
            //Stop the first instance of the clip we come across
            if (audioSource.clip == SoundMap[effect].Clip)
            {
                audioSource.Stop();
                return;
            }
        }
    }

    public void StopAll()
    {
        foreach (var audioSource in _sources)
        {
            audioSource.Stop();
        }
    }

    public class Sound
    {
        private const string SoundPath = "Sounds/";
        public Sound(string name, float volume = 1f, float startTime = 0f)
        {
            Path = SoundPath + name;
            Volume = volume;
            StartTime = startTime;
        }
        public void LoadClip()
        {
            Clip = Resources.Load(Path) as AudioClip;
        }

        public readonly float Volume;
        public readonly float StartTime;
        public string Path;
        public AudioClip Clip;
    }
}
