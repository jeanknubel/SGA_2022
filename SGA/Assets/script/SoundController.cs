using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioClip background;
    [SerializeField]
    private List<AudioClip> darumaGrognement, chute, empereurColere, empereurGrognement, empereurMange, jump, lancer, prendreAliment, victory;
    private List<List<AudioClip>> clips;
    private AudioSource sourceBackGround;
    private AudioSource sourceEffects;
    public enum Sound
    {
        DARUMA_GROGNEMENT, CHUTE, EMPEREUR_COLERE, EMPEREUR_GROGNEMENT, EMPEREUR_MANGE, JUMP, LANCER, PRENDRE_ALIMENT, VICTORY
    }
    private void Awake()
    {
        sourceBackGround = gameObject.AddComponent<AudioSource>();
        sourceEffects = gameObject.AddComponent<AudioSource>();
        clips = new List<List<AudioClip>>();
        clips.Add(darumaGrognement);
        clips.Add(chute);
        clips.Add(empereurColere);
        clips.Add(empereurGrognement);
        clips.Add(empereurMange);
        clips.Add(jump);
        clips.Add(lancer);
        clips.Add(prendreAliment);
        clips.Add(victory);
    }

    public void playSound(Sound sound)
    {
            List<AudioClip> selectedClips = clips[(int)sound];
            int index = Random.Range(0, selectedClips.Count);
            sourceEffects.clip = selectedClips[index];
            sourceEffects.Play();
    }
    public void playBackground()
    {
        sourceBackGround.clip = background;
        sourceBackGround.loop = true;
        sourceBackGround.Play();
    }

}
