using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    public SoundSO[] sounds;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }


        foreach (var sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;

            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
           // sound.audioSource.Play();
        }
    }

    public void PlaySound(string soundName)
    {
        SoundSO s = System.Array.Find(sounds, sound => sound.soundName == soundName);
        if (s == null) {
            Debug.Log("Sound is not found");
            return; }
        else
        {
            s.audioSource.Play();
        }
       
    }

    public void StopSound(string soundName)
    {
        SoundSO s = System.Array.Find(sounds, sound => sound.soundName == soundName);
        if (s == null)
        {
            Debug.Log("Sound is not found");
            return;
        }
        s.audioSource.Stop();
    }
}
