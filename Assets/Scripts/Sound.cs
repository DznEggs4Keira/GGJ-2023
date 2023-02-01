using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [Header("Audio File")]
    public string name;
    public AudioClip clip;

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float volume;
    public bool loop;
    public AudioMixerGroup audioMixerGroup;

    [HideInInspector]
    public AudioSource audioSource;
}
