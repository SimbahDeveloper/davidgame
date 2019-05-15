using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioHelper : MonoBehaviour
{
    [Header("AudioSource")]
    public AudioSource SFX;
    public AudioSource Music;

    [Header("Audio Clip")]
    public AudioClip Click;
    public AudioClip MenuMusic;

    public float GetVolumeSFX()
    {
        return SFX.volume;
    }
    public float GetVolumeMusic()
    {
        return Music.volume;
    }

    public void PlayClick()
    {
        SFX.clip = Click;
        SFX.Play();
    }

    public static int MENUMUSIC = 1;
    public void PlayMusic(int i)
    {
        switch (i)
        {
            case 1:
                Music.clip = MenuMusic;
                Music.Play();
                break;
            default:
                break;
        }

    }
    public void SetSFXVolume(float val)
    {
        SFX.volume = val;
    }

    public void SetMusicVolume(float val)
    {
        Music.volume = val;
    }
}
