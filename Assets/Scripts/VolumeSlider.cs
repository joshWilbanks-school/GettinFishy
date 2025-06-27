using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] VolumeManager volumeManager;
    [SerializeField] AudioChannel channel;

    private void Start()
    {
        float value = GetComponent<Slider>().value;

        if (channel.Equals(AudioChannel.Master))
            volumeManager.SetMasterVolume(value);
        else if (channel.Equals(AudioChannel.Music))
            volumeManager.SetMusicVolume(value);
        else if (channel.Equals(AudioChannel.SFX))
            volumeManager.SetSfxVolume(value);
    }
}
