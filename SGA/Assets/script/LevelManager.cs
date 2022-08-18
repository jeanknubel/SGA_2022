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
    private GameObject foodFallManager, SoundController;


    void Start()
    {
        timeRemaining = 120;
        StartCoroutine(startTimer());
    }

    IEnumerator startTimer()
    {
        yield return new WaitForSeconds(1);
        print(3);
        yield return new WaitForSeconds(1);
        print(2);
        yield return new WaitForSeconds(1);
        print(1);
        yield return new WaitForSeconds(1);
        launchGame();
        StartCoroutine(GameTimer());

    }

    IEnumerator GameTimer()
    {
        while(timeRemaining > 0)
        {
            timeRemaining--;
            yield return new WaitForSeconds(1);
            print("time remaining: " + timeRemaining);
        }
    }

    private void launchGame()
    {
        foreach(GameObject player in players)
        {
            player.GetComponent<PlayerInputController>().startGame();
        }
        foodFallManager.GetComponent<FoodFall>().startGame();
        SoundController.GetComponent<SoundController>().startFight();
    }
}
