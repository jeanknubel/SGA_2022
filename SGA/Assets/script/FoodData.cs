using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : MonoBehaviour
{
    private FoodStats stats;
    public void setStats(FoodStats stats)
    {
        this.stats = stats;
    }
    public int getScore()
    {
        return stats.getScore();
    }
}
