using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoodInteraction : MonoBehaviour
{
    private bool holdFood;
    private GameObject food;
    [SerializeField]
    private float massThrownObject, forceThrownObject, gravityThrownObject;

    private void Start()
    {
        holdFood = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if(other.layer == LayerMask.NameToLayer("Food"))
        {
            if (food == null)
            {
                food = other;
                other.transform.parent = transform;
                other.transform.localPosition = Vector2.up;
                other.transform.rotation = Quaternion.identity;
                other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                other.GetComponent<Rigidbody2D>().freezeRotation = true;
            }
        }
    }

    public void throwFood(Vector2 direction)
    {
        if(food != null)
        {
            food.transform.parent = null;
            food.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            food.GetComponent<Rigidbody2D>().AddForce(direction * forceThrownObject, ForceMode2D.Impulse);
            food.GetComponent<Rigidbody2D>().mass = massThrownObject;
            food.GetComponent<Rigidbody2D>().gravityScale = gravityThrownObject;
            food = null;
        }
    }
    public void looseFood()
    {
        Destroy(food);
        food = null;
    }
}
