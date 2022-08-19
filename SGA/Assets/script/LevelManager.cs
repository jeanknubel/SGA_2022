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
        launchGame();
        StartCoroutine(GameTimer());
        timerTxt.text = timeRemaining.ToString();
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
    }

}
