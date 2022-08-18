using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    private bool startingGame = false;
    private int timeRemaining;

    [SerializeField]
    private List<GameObject> players;
    [SerializeField]
    private GameObject foodFallManager, SoundController, vieuxMan;


    void Start()
    {
        timeRemaining = 120;
        StartCoroutine(startTimer());
    }

    IEnumerator startTimer()
    {
        vieuxMan.GetComponent<Animator>().Play("VieuxMan_Gong");
        yield return new WaitForSeconds(2);
        vieuxMan.SetActive(false);
        launchGame();
        StartCoroutine(GameTimer());

    }

    IEnumerator GameTimer()
    {
        while(timeRemaining > 0)
        {
            timeRemaining--;
            yield return new WaitForSeconds(1);
            //print("time remaining: " + timeRemaining);
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
