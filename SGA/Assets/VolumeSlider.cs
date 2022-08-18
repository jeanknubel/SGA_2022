using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void Setvolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

}
