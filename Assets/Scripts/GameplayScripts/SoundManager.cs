using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    public AudioSource soundFxManager, soundFxManager2;

    public AudioSource themeSongManager;

    public AudioClip[] themeSongs;



    void Awake()
    {
        MakeSingleton();
    }

    void MakeSingleton()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

   

    void Update()
    {
        if (!themeSongManager.isPlaying)
        {
            themeSongManager.clip = themeSongs[Random.Range(0, themeSongs.Length)];
            themeSongManager.Play();
        }
    }

    public void PlaySoundFx(AudioClip audioClip, float volume)
    {
        if (!soundFxManager.isPlaying)
        {
            soundFxManager.clip = audioClip;
            soundFxManager.volume = volume;
            soundFxManager.Play();
        }
        else
        {
            soundFxManager2.clip = audioClip;
            soundFxManager2.volume = volume;
            soundFxManager2.Play();
        }
    }

    public void PlayRandomSOundFx(AudioClip[] audioClips)
    {
        if (!soundFxManager.isPlaying)
        {
            soundFxManager.clip = audioClips[Random.Range(0, audioClips.Length)];
            soundFxManager.volume = Random.Range(.2f, .6f);
            soundFxManager.Play();
        }
    }
}
