using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioClip background, vieuxGong;
    [SerializeField]
    private List<AudioClip> darumaGrognement, chute, empereurColere, empereurGrognement,
                        empereurMange, jump, lancer, prendreAliment, victory, gong, collision, sautAliment, stempfel, timer, fxEmpereur;
    private List<List<AudioClip>> clips;
    private AudioSource sourceBackGround;
    private AudioSource sourceEffects, sourceFx;
    public enum Sound
    {
        DARUMA_GROGNEMENT, CHUTE, EMPEREUR_COLERE, EMPEREUR_GROGNEMENT, EMPEREUR_MANGE, JUMP, LANCER, PRENDRE_ALIMENT, VICTORY, GONG, COLLISION, SAUT_ALIMENT,
        STEMPFEL, TIMER, FX_EMPEREUR
    }
    private void Awake()
    {
        sourceBackGround = gameObject.AddComponent<AudioSource>();
        sourceEffects = gameObject.AddComponent<AudioSource>();
        sourceFx = gameObject.AddComponent<AudioSource>();

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
        clips.Add(gong);
        clips.Add(collision);
        clips.Add(sautAliment);
        clips.Add(stempfel);
        clips.Add(timer);
        clips.Add(fxEmpereur);
    }

    public void playSound(Sound sound)
    {
            List<AudioClip> selectedClips = clips[(int)sound];
            int index = Random.Range(0, selectedClips.Count);
            sourceEffects.clip = selectedClips[index];
            sourceEffects.Play();
    }
    public void playSoundFx(Sound sound)
    {
        List<AudioClip> selectedClips = clips[(int)sound];
        int index = Random.Range(0, selectedClips.Count);
        sourceFx.clip = selectedClips[index];
        sourceFx.Play();
    }
    public void playBackground()
    {
        sourceBackGround.clip = background;
        sourceBackGround.loop = true;
        sourceBackGround.Play();
    }
    public void startGame()
    {
        sourceEffects.clip = vieuxGong;
        sourceEffects.Play();
    }

}
