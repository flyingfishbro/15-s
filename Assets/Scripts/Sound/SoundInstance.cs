using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInstance : MonoBehaviour
{    protected virtual void InitializeSounds() { }
    protected virtual void DestroySounds() { }

    protected void AddSound(string code, string path)
    {
        AudioSource prefab = Resources.Load<AudioSource>(path);
        AudioSource source = Instantiate(prefab, SoundManager.Instance.transform);
        if (!SoundManager.Instance.TryAddSound(code, source))
        {
            Destroy(source);
        }
    }

    protected void RemoveSound(string code)
    {
        SoundManager.Instance.RemoveSound(code);
    }

    private void Start()
    {
        InitializeSounds();
    }
}