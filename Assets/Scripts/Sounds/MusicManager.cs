using System.Collections.Generic;
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
        Mountain,
        Maze,
        Desert
    }

    private Dictionary<Song, string> SongMap = new Dictionary<Song, string>()
    {
        {Song.OffAir, "Music/Off-Air Prepare"},
        {Song.HubOutside, "Music/Heroes' Hub"},
        {Song.HubTower, "Music/Heroes' Hub (Tick)"}
    };

    void Awake()
    {
        var audioSource = gameObject.AddComponent<AudioSource>();
        var clip = Resources.Load(SongMap[Song.HubTower]) as AudioClip;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
