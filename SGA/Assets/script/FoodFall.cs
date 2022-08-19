using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFall : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float foodSpawnProbability;

    [SerializeField]
    private Transform up;
    [SerializeField]
    private float foodSpawnRadius, spawnRate, gravityScale;

    [SerializeField]
    private GameObject foodPrefab;

    [SerializeField]
    private List<FoodStats> foodTypes;

    [SerializeField]
    private GameObject particleSystemPrefab;

    private List<float> tresh = new List<float>();
    private float totTresh = 0;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (FoodStats foodStat in foodTypes)
        {
            totTresh += foodStat.getRarity();
            tresh.Add(totTresh);
        }       
    }
    public void startGame()
    {
        StartCoroutine(foodSpawnCoroutine());
    }

    IEnumerator foodSpawnCoroutine()
    {
        while (true)
        {
            if(Random.value < foodSpawnProbability)
            {
                spawnFood();
            }
            yield return new WaitForSecondsRealtime(spawnRate);
        }

    }

    private void spawnFood()
    {
        float chooseFood = Random.Range(0, totTresh);
        int elementIdx = 0;
        foreach(float treshold in tresh)
        {
            if (chooseFood <= treshold) break;
            elementIdx++;
        }

        FoodStats foodType = foodTypes[elementIdx];

        Vector2 position = new Vector2(Random.Range(-foodSpawnRadius, foodSpawnRadius), up.position.y);
        GameObject food = Instantiate(foodPrefab, position, Quaternion.identity);
        food.layer = LayerMask.NameToLayer("Food");
        food.GetComponent<SpriteRenderer>().sprite = foodType.getSprite();
        food.GetComponent<Rigidbody2D>().gravityScale = 0.3f;
        food.AddComponent<FoodData>();
        food.GetComponent<FoodData>().setStats(foodType);

        if (foodType.getScore() < 0)
        {
            GameObject particlePrefab = Instantiate(particleSystemPrefab, food.transform);
            food.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            food.transform.GetChild(0).gameObject.AddComponent<SineWaveRenderer>();
            food.transform.GetChild(0).gameObject.GetComponent<SineWaveRenderer>().GlowColor = Color.yellow;
            food.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = foodTypes[elementIdx].getSpriteGlow();
        }


        //SpriteRenderer glowing = food.GetComponentsInChildren<SpriteRenderer>()[1];
        //glowing.sprite = foodType.getSpriteGlow();
        //glowing.color = new Color(1, 0.92f, 0.016f, foodType.getScore() == 5 ? 1 : foodType.getScore() > 1 ? 0.4f : 0);
        //if (foodType.getScore() == 5) glowing.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

    }
}
