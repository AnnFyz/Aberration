using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/Sound", order = 1)]
public class SoundSO : ScriptableObject
{
    public string soundName;
    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;
    [HideInInspector]
    public AudioSource audioSource;
}
