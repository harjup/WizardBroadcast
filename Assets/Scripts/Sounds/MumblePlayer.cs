using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameState;
using UnityEngine;

//Initally was just for dialog mumble, since the logic is similar I'm going to put walk cycle noises in here as well.
//TODO: Etiehr genericize the logic in here, or separate out the walk logic to its own thing
public class MumblePlayer : Singleton<MumblePlayer>
{
    public enum WalkType 
    {
        Undefined,
        Grass,
        Stone,
        Sand,
    }

    private Dictionary<MumbleType, string> _mumblePaths = new Dictionary<MumbleType, string>()
    {
        {MumbleType.Wizard, "Sounds/Mumble/Wizard"},
        {MumbleType.Sign,   "Sounds/Mumble/Sign"},
        {MumbleType.Jeer,   "Sounds/Mumble/Jeer"}
    };

    private Dictionary<WalkType, string> _walkPaths = new Dictionary<WalkType, string>()
    {
        {WalkType.Grass, "Sounds/Walk/Grass"},
        {WalkType.Stone,   "Sounds/Walk/Stone"},
        {WalkType.Sand,   "Sounds/Walk/Sand"}
    };

    private Dictionary<MumbleType, AudioClip[]> _mumbleMap 
        = new Dictionary<MumbleType, AudioClip[]>();

    private Dictionary<WalkType, AudioClip[]> _walkMap
        = new Dictionary<WalkType, AudioClip[]>();

    private AudioSource _source;
    private AudioSource _walkSource;

    // Use this for initialization
    void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
        _walkSource = gameObject.AddComponent<AudioSource>();

        foreach (var mumblePath in _mumblePaths)
        {
            var mumbleSet = Resources.LoadAll(mumblePath.Value).Cast<AudioClip>().ToArray();

            if (mumbleSet.Length == 0)
            {
                Debug.LogError("Unable to load mumble clips for " + mumblePath.Key);
            }
            _mumbleMap.Add(mumblePath.Key, mumbleSet);
        }

        foreach (var walkPath in _walkPaths)
        {
            var walkSet = Resources.LoadAll(walkPath.Value).Cast<AudioClip>().ToArray();

            if (walkSet.Length == 0)
            {
                Debug.LogError("Unable to load walk clips for " + walkPath.Key);
            }
            _walkMap.Add(walkPath.Key, walkSet);
        }

    }

    void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == SceneMap.GetScene(Scene.Start))
        {
            StopMumble();
            StopFootsteps();
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
        if (_mumbleRoutine == null) return;
        StopCoroutine(_mumbleRoutine);
        _source.Stop();
    }


    private IEnumerator _walkRoutine;
    public void PlayFootsteps(WalkType type)
    {
        if (_walkRoutine != null) return;

        _walkRoutine = WalkLoop(type);
        StartCoroutine(_walkRoutine);
    }

    IEnumerator WalkLoop(WalkType type)
    {
        _walkSource.volume = 1f;

        while (true)
        {
            var clips = _walkMap[type].AsRandom().ToArray();
            var cutOffMultiplier = 1f;
            var delayAmount = 0f;
            if (type == WalkType.Grass)
            {
                cutOffMultiplier = .5f;
                _walkSource.volume = .50f;
            }
            else if (type == WalkType.Stone)
            {
                delayAmount = .1f;
                _walkSource.volume = .15f;
            }

            foreach (var clip in clips)
            {
                _walkSource.clip = clip;
                _walkSource.loop = false;
                _walkSource.Play();


                //There's some silence at the end of each walk clip, so we need to cut it off a bit early
                while (_walkSource.time <= clip.length * cutOffMultiplier)
                {
                    if (!_walkSource.isPlaying) break;

                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(delayAmount);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void StopFootsteps()
    {
        if (_walkRoutine == null) return;
        StopCoroutine(_walkRoutine);
        _walkRoutine = null;
        _walkSource.Stop();
    }

}
