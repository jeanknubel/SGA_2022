using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : MonoBehaviour
{
    private FoodStats stats;
    private GameObject owner ;
    public void setStats(FoodStats stats)
    {
        this.stats = stats;
        owner = null;
    }
    public int getScore()
    {
        return stats.getScore();
    }
    public void setOwner(GameObject owner) { this.owner = owner; }
    public GameObject getOwner() { return owner; }
}
