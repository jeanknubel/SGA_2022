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

    private List<float> tresh = new List<float>();
    private float totTresh = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (FoodStats foodStat in foodTypes)
        {
            totTresh += foodStat.getRarity();
            tresh.Add(totTresh);
        }
            
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

        Vector2 position = new Vector2(Random.Range(-foodSpawnRadius, foodSpawnRadius), up.position.y);
        GameObject food = Instantiate(foodPrefab, position, Quaternion.identity);
        food.layer = LayerMask.NameToLayer("Food");
        food.GetComponent<SpriteRenderer>().sprite = foodTypes[elementIdx].getSprite();
        food.GetComponent<Rigidbody2D>().gravityScale = 0.3f;
    }
}