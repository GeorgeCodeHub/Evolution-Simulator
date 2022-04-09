using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioMixer soundMixer;

   public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Music",volume);
    }

    public void SetSound(float sound)
    {
        soundMixer.SetFloat("Sound", sound);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
