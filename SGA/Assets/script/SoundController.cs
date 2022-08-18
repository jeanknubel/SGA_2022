using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioClip soudLevel;
    private AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void startFight()
    {
        source.clip = soudLevel;
        source.Play();
        
    }
}
