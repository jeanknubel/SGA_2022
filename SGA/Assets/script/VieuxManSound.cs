using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VieuxManSound : MonoBehaviour
{
    [SerializeField]
    private SoundController soundController;
    public void playSound()
    {
        soundController.startGame();
    }
    public void playBackGround()
    {
        soundController.playBackground();
    }
    public void desactivate()
    {
        gameObject.SetActive(false);
    }
}
