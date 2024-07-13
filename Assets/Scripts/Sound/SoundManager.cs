using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Audio;

public enum ESoundType
{
    Master,
    BGM,
    SFX,
}


public class SoundManager : MonoBehaviour
{
    #region VOLUME_FIELD    
    [SerializeField]
    private AudioMixer _mixer;

    private float _masterVolume = 1.0f;
    public float MasterVolume => _masterVolume;
    private float _bgmVolume = 1.0f;
    public float BgmVolume => _bgmVolume;
    private float _sfxVolume = 1.0f;
    public float SfxVolume => _sfxVolume;
    #endregion

    #region SOUNDS_FIELD
    private Dictionary<string, AudioSource> _sounds = new Dictionary<string, AudioSource>();
    #endregion

    #region MANAGER
    private static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _mixer = Resources.Load<AudioMixer>("Sound/MasterAudioMixer");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    #region VOLUME
    public void SetVolume(ESoundType type, float volume)
    {
        switch (type)
        {
            case ESoundType.Master:
                SetMasterVolume(volume);
                return;
            case ESoundType.BGM:
                SetBgmVolume(volume);
                return;
            case ESoundType.SFX:
                SetSfxVolume(volume);
                return;
        }
    }

    public void SetMasterVolume(float volume)
    {
        _mixer.SetFloat("Master", volume.ToLogarithmicVolume());
        _masterVolume = volume;
    }

    public void SetBgmVolume(float volume)
    {
        _mixer.SetFloat("BGM", volume.ToLogarithmicVolume());
        _bgmVolume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        _mixer.SetFloat("SFX", volume.ToLogarithmicVolume());
        _sfxVolume = volume;
    }
    #endregion

    #region SOUND
    public bool TryAddSound(string code, AudioSource sound)
    {
        if (!_sounds.ContainsKey(code))
        {
            _sounds.Add(code, sound);
            return true;
        }

        return false;
    }

    public void AddSound(string code, AudioSource sound)
    {
        TryAddSound(code, sound);
    }

    public void RemoveSound(string code)
    {
        if (_sounds.ContainsKey(code))
        {
            AudioSource sound = _sounds[code];
            Destroy(sound.gameObject);
            _sounds.Remove(code);
        }
    }

    public void Play(string code, float time = 0.0f)
    {
        if (_sounds.TryGetValue(code, out AudioSource sound))
        {
            sound.Play();
            sound.time = time;
            return;
        }

        Debug.LogError($"Failed to play sound : {code}", this);
    }

    public float Stop(string code)
    {
        if (_sounds.TryGetValue(code, out AudioSource sound) && sound.isPlaying)
        {
            float playedTime = sound.time;

            sound.Stop();
            return playedTime;
        }

        return 0.0f;
    }
    #endregion 
}
