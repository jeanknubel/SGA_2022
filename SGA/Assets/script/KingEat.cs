using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingEat : MonoBehaviour
{
    //king eat the food
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == LayerMask.NameToLayer("FoodForKing")) Destroy(other);
    }
}
