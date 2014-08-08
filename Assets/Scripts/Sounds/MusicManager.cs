using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.GameState;
using UnityEngine;
using System.Collections;

public class MusicManager : Singleton<MusicManager>
{
    public enum Song
    {
        Undefined,
        OffAir,
        HubOutside,
        HubTower,
        Forest,
        MountainHot,
        MountainCold,
        Maze,
        Desert
    }

    private Dictionary<Scene, Song[]> _sceneToSongMap = new Dictionary<Scene, Song[]>()
    {
        {Scene.Start,   new[] {Song.OffAir}},
        {Scene.Hub,     new[] {Song.HubOutside, Song.HubTower}},
        {Scene.Level1,  new[] {Song.Forest}},
        {Scene.Level2,  new[] {Song.MountainHot, Song.MountainCold}},
        {Scene.Level3,  new[] {Song.Maze}},
        {Scene.Level4,  new[] {Song.Desert}}
    };

    private readonly Dictionary<Song, string> _songMap = new Dictionary<Song, string>()
    {
        {Song.OffAir, "Music/Off-Air Prepare"},
        {Song.HubOutside, "Music/Heroes' Hub"},
        {Song.HubTower, "Music/Heroes' Hub (Tick)"},
        {Song.Forest, "Music/Farcical Forest (v1)"},
        {Song.MountainHot, "Music/Burning Below Zero - Heated (v1)"},
        {Song.MountainCold, "Music/Burning Below Zero - Cooled (v1)"},
        {Song.Maze, "Music/Questionable Current (v1)"},
        {Song.Desert, "Music/Heroes' Hub (Tick)"}
    };

    private AudioSource _musicSource;
    private Song _primarySong;
    private AudioSource _secondarySource;
    private Song _secondarySong;

    void Awake()
    {
        _musicSource =       gameObject.AddComponent<AudioSource>();
        _secondarySource =   gameObject.AddComponent<AudioSource>();
        LoadSongForLevel();
    }

    void OnLevelWasLoaded(int level)
    {
        _musicSource.volume = .5f;
        _secondarySource.volume = 0f;
        LoadSongForLevel();
    }

    void LoadSongForLevel()
    {
        Song[] songs = _sceneToSongMap[SceneMap.GetSceneFromStringName(Application.loadedLevelName)];
        _primarySong = songs[0];
        var clip = Resources.Load(_songMap[_primarySong]) as AudioClip;
        _musicSource.clip = clip;
        _musicSource.loop = true;

        //Let's support up to 2 concurrent songs per level for now, can have more eventually I just don't feel like doing this with arrays
        if (songs.Length > 1)
        {
            _secondarySong = songs[1];
            var altClip = Resources.Load(_songMap[_secondarySong]) as AudioClip;
            _secondarySource.clip = altClip;
            _secondarySource.loop = true;
            _secondarySource.volume = 0f;
            _secondarySource.Play();
        }
        _musicSource.Play();
    }

    //Only supports 2 songs until I decide to have more than two for any given level 
    public void TransitionSongs(int index)
    {
        if (index == 0 && _secondarySource.volume > 0f)
        {
            StartCoroutine(VolumeTransition(_musicSource, .5f));
            StartCoroutine(VolumeTransition(_secondarySource, 0f));
            
        }
        else if (index == 1 && _musicSource.volume > 0f)
        {
            StartCoroutine(VolumeTransition(_musicSource, 0f));
            StartCoroutine(VolumeTransition(_secondarySource, .5f));
        }
    }

    IEnumerator VolumeTransition(AudioSource source, float target)
    {
        while (Mathf.Abs(source.volume - target) >= .01f)
        {
            source.volume = iTween.FloatUpdate(source.volume, target, 2f);
            yield return new WaitForEndOfFrame();
        }
        source.volume = target;
    }

    public bool SoundEnabled
    {
        get { return Math.Abs(AudioListener.volume - 1f) < .05; }
        set { AudioListener.volume = value ? 1 : 0; }
    }
}
