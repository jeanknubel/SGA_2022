using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

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
    [SerializeField]
    private SpriteRenderer fondNoir;
    [SerializeField]
    private Image[] roundMarkBleu, roundMarkRouge;
    [SerializeField]
    private Sprite markBleuSprite, markRougeSprite;
    [SerializeField]
    private GameObject feu;
    [SerializeField]
    private SoundController soundController;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private VideoPlayer videoDebut;
    [SerializeField]
    private GameObject videoRenderer;
    [SerializeField]
    private Queue<Image> roundMarkBleuQueue, roundMarkRougeQueue;
    [SerializeField]
    private GameObject darumaSelect;
    private bool videoPlaying;

    void Start()
    {
        darumaSelect.SetActive(false);
        videoDebut.Play();
        videoPlaying = true;
        roundMarkBleuQueue = new Queue<Image>();
        roundMarkRougeQueue = new Queue<Image>();
        foreach (Image o in roundMarkBleu) roundMarkBleuQueue.Enqueue(o);
        foreach (Image o in roundMarkRouge) roundMarkRougeQueue.Enqueue(o);
    }

    private void selectDaruma()
    {
        videoRenderer.SetActive(false);
        darumaSelect.SetActive(true);
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerInputController>().selectDaruma();
        }

    }

    IEnumerator GameTimer()
    {
        while(timeRemaining > 0)
        {
            if(timeRemaining <= 3)
            {
                soundController.playSoundFx(global::SoundController.Sound.TIMER);
                timerTxt.color = Color.red;
            }
            timeRemaining--;
            timerTxt.text = timeRemaining.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        endRound();
    }

    private void endRound()
    {
        soundController.playSoundFx(global::SoundController.Sound.GONG);
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerInputController>().endRound();
        }
        foodFallManager.GetComponent<FoodFall>().stopGame();
        StartCoroutine(fadeToBlack());
        Invoke("markWinner", 2);

    }

    IEnumerator fadeToBlack()
    {
        for(float c = 0; c < 0.7f; c+= 0.05f)
        {
            Color color = fondNoir.color;
            color.a = c;
            fondNoir.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void markWinner()
    {
        soundController.playSound(global::SoundController.Sound.STEMPFEL);
        int score1 = players[0].GetComponent<PlayerFoodInteraction>().getScore();
        int score2 = players[1].GetComponent<PlayerFoodInteraction>().getScore();
        if (score1 > score2)
        {
            roundMarkBleuQueue.Dequeue().sprite = markBleuSprite;
            print("bleu win");
        }
        else if (score2 > score1)
        {
            roundMarkRougeQueue.Dequeue().sprite = markRougeSprite;
            print("rouge win");
        }
        else
        {
            roundMarkBleuQueue.Dequeue().sprite = markBleuSprite;
            roundMarkRougeQueue.Dequeue().sprite = markRougeSprite;
        }


        if(roundMarkBleuQueue.Count == 0 || roundMarkRougeQueue.Count == 0)
        {
            print("end party");
        }
        else
        {
            Invoke("launchGame", 2f);
        }
    }
    IEnumerator fadeFromBlack()
    {
        print(fondNoir.color.a);
        Color color = new Color(0, 0, 0, 0.7f);
        for (float c = 0.7f; c >= 0f; c -= 0.05f)
        {
            color.a = c;
            fondNoir.color = color;
            yield return new WaitForSeconds(0.05f);
        }
        print(fondNoir.color.a);

    }
    public void pauseGame()
    {
        StartCoroutine(fadeToBlack());
        pauseMenu.SetActive(true);

    }
    public bool isVideoPlaying() { return videoPlaying; }
    public void stopVideo()
    {
        videoDebut.Stop();
        selectDaruma();
        videoPlaying = false;
    }
    public void resumeGame()
    {
        StartCoroutine(fadeFromBlack());
        pauseMenu.SetActive(false);
    }
    public void launchGame()
    {
        StartCoroutine(_launchGame());
    }
    IEnumerator _launchGame()
    {
        vieuxMan.SetActive(true);
        videoPlaying = false;
        darumaSelect.SetActive(false);

        yield return  new WaitForSeconds(2);
        soundController.playBackground();
        pauseMenu.SetActive(false);
        timerTxt.color = new Color(0.9921569f, 0.9529412f, 0.8509805f, 1);

        print("begin");
        feu.SetActive(false);
        timeRemaining = 20;
        timerTxt.text = timeRemaining.ToString();


        StartCoroutine(fadeFromBlack());
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerInputController>().startGame();
            player.GetComponent<PlayerFoodInteraction>().startGame();

        }
        foodFallManager.GetComponent<FoodFall>().startGame();
        StartCoroutine(GameTimer());
    }

}
