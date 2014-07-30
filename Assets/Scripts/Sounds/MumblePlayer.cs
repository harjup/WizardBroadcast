using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MumblePlayer : Singleton<MumblePlayer>
{
    

    private Dictionary<MumbleType, string> _mumblePaths = new Dictionary<MumbleType, string>()
    {
        {MumbleType.Wizard, "Sounds/Mumble/Wizard"},
        {MumbleType.Sign,   "Sounds/Mumble/Sign"},
        {MumbleType.Jeer,   "Sounds/Mumble/Jeer"}
    };

    private Dictionary<MumbleType, AudioClip[]> _mumbleMap 
        = new Dictionary<MumbleType, AudioClip[]>();

    private AudioSource _source;

    // Use this for initialization
    void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();

        foreach (var mumblePath in _mumblePaths)
        {
            var mumbleSet = Resources.LoadAll(mumblePath.Value).Cast<AudioClip>().ToArray();

            if (mumbleSet.Length == 0)
            {
                Debug.LogError("Unable to load mumble clips for " + mumblePath.Key);
            }
            _mumbleMap.Add(mumblePath.Key, mumbleSet);
        }
    }

    private IEnumerator _mumbleRoutine;
    public void PlayMumble(MumbleType type)
    {
        _mumbleRoutine = MumbleLoop(type);
        StartCoroutine(_mumbleRoutine);
    }

    IEnumerator MumbleLoop(MumbleType type)
    {
        while (true)
        {
            var clips = _mumbleMap[type].AsRandom().ToArray();
            foreach (var clip in clips)
            {
                _source.clip = clip;
                _source.loop = false;
                _source.Play();

                while (_source.isPlaying)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(.2f);
            }
            yield return new WaitForEndOfFrame();
        }
    }


    public void StopMumble()
    {
        StopCoroutine(_mumbleRoutine);
        _source.Stop();
    }
}
