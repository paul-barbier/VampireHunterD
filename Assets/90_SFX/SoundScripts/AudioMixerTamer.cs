using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerTamer : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;
    [SerializeField] private string name;

    public void SetVolume(float sliderValue)
    {
        myAudioMixer.SetFloat(name, Mathf.Log10(sliderValue) * 20);
    }
}

