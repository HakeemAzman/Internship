using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClips[] clips;

    public static SoundManager instance;

    public bool dontDestroyOnLoad = false;

    public void Awake()
    {
        if(dontDestroyOnLoad)
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        foreach (AudioClips ac in clips)
        {
           ac.audioSource = gameObject.AddComponent<AudioSource>();

           ac.audioSource.clip = ac.audioClip; 
           ac.audioSource.volume = ac.volume;
           ac.audioSource.pitch = ac.pitch;
           ac.audioSource.loop = ac.loop;
        }
    }

    public void Play(string name)
    {
       AudioClips ac =  Array.Find(clips, sound => sound.name == name);

        if(ac == null)
        {
            Debug.LogWarning("AudioClip: " + name + " not located! :(");
            return;
        }

       ac.audioSource.Play();
    }

    public void Stop(string name)
    {
        AudioClips ac = Array.Find(clips, sound => sound.name == name);

        if (ac == null)
        {
            Debug.LogWarning("AudioClip: " + name + " not located! :(");
            return;
        }

        ac.audioSource.Stop();
    }
}
