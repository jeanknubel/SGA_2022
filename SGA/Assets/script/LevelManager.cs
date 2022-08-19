using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    private bool startingGame = false;
    private int timeRemaining;

    [SerializeField]
    private List<GameObject> players;
    [SerializeField]
    private GameObject foodFallManager, SoundController, vieuxMan;
    [SerializeField]
    private TextMeshProUGUI timerTxt;
    [SerializeField]
    private float vieuxManSoundDelay;



    void Start()
    {

        timeRemaining = 120;
        StartCoroutine(startTimer());
        timerTxt.text = timeRemaining.ToString();
    }

    IEnumerator startTimer()
    {
        yield return new WaitForSeconds(vieuxManSoundDelay);
        SoundController.GetComponent<SoundController>().startGame();
        yield return new WaitForSeconds(1.6f);
        vieuxMan.SetActive(false);
        launchGame();
        StartCoroutine(GameTimer());

    }


    IEnumerator GameTimer()
    {
        while(timeRemaining > 0)
        {
            timeRemaining--;
            timerTxt.text = timeRemaining.ToString();
            yield return new WaitForSeconds(1);
        }
    }

    private void launchGame()
    {
        foreach(GameObject player in players)
        {
            player.GetComponent<PlayerInputController>().startGame();
        }
        foodFallManager.GetComponent<FoodFall>().startGame();
        SoundController.GetComponent<SoundController>().playBackground();
    }

}
