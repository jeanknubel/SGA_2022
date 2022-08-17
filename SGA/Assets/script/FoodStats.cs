using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Food")]
public class FoodStats : ScriptableObject
{
    [SerializeField]
    private int score;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private int rarity;

    public int getScore() { return score; }
    public Sprite getSprite() { return sprite; }
    public int getRarity() { return rarity; }
}
