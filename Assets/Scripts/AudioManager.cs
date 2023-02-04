using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;
 
    private void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        // in order to make the manager persist between scenes
        DontDestroyOnLoad(gameObject);

        foreach (var sound in sounds) {
            // Assign each sound an audio source
            sound.audioSource = gameObject.AddComponent<AudioSource>();

            // Set the parameters of each audio source
            sound.audioSource.clip = sound.clip;
            sound.audioSource.outputAudioMixerGroup = sound.audioMixerGroup;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        Play("Gameplay Music");
        //Play("Bird Noise");
    }

    public void Play(string name, bool isSFX = false) {

        Sound soundToPlay = Array.Find(sounds, sound => sound.name == name);

        if (soundToPlay != null) {

            // Set a random Pitch value
            if(isSFX) {
                soundToPlay.audioSource.pitch = UnityEngine.Random.Range(soundToPlay.minPitch, soundToPlay.maxPitch); 
            }
            soundToPlay.audioSource.Play();
        
        } else {
            Debug.Log($"Sound {name} not found");
        }
        
    }

    // check if it is moving footsteps, doin't spam play it if already playing
    public void ReccuringPlay(string name, bool isMoving = false) {

        Sound footsteps = Array.Find(sounds, sound => sound.name == name);

        if (isMoving) {
            if (!footsteps.audioSource.isPlaying) {

                // Set random value between max pitch and min pitch
                footsteps.audioSource.pitch = UnityEngine.Random.Range(footsteps.minPitch, footsteps.maxPitch);
                footsteps.audioSource.Play();
            }
        } else {
            footsteps.audioSource.Stop();
        }
       

        
    }

    // stop all sfx on pause
    public void StopAllSFX() {
        foreach (var sound in sounds) {
            if (sound.audioMixerGroup.name == "SFX") {
                sound.audioSource.Stop();
            }
        }
    }
}
