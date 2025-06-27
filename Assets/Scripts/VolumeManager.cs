using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;


    public void Start()
    {
        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSfxVolume(sfxSlider.value);
    }

    public void SetMasterVolume(float volume)
    {
        if(volume < -50)
            volume = -80;
        audioMixer.SetFloat("Master", volume);
    }

    public void SetMusicVolume(float volume)
    {
        if (volume < -50)
            volume = -80;
        audioMixer.SetFloat("Music", volume);
    }

    public void SetSfxVolume(float volume)
    {
        if (volume < -50)
            volume = -80;
        audioMixer.SetFloat("SFX", volume);
    }

}
