using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerFoodInteraction : MonoBehaviour
{
    private bool holdFood;
    private GameObject food;
    [SerializeField]
    private float massThrownObject, forceThrownObject, gravityThrownObject, throwPowerMultiplier, throwTime;
    [SerializeField]
    private string throwAnimation;
    [SerializeField]
    private SoundController soundController;
    [SerializeField]
    private TextMeshProUGUI scoreTxt;

    private float timer;
    private bool countTime;
    private PlayerInputReceiver receiver;
    private PlayerInputController controller;
    private Rigidbody2D foodRb;
    private bool hasTrown;
    private int score;

    public bool isHoldingFood() { return holdFood; }
    private void Start()
    {
        holdFood = false;
        countTime = false;
        food = null;
        timer = 0;
        receiver = GetComponent<PlayerInputReceiver>();
        controller = GetComponent<PlayerInputController>();
        score = 0;
        scoreTxt.text = "0";
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if(other.layer == LayerMask.NameToLayer("Food"))
        {
            if (!holdFood && food == null)
            {
                soundController.playSound(SoundController.Sound.PRENDRE_ALIMENT);
                food = other;
                food.transform.parent = transform;
                food.transform.localPosition = Vector2.up * 0.75f;
                food.transform.rotation = Quaternion.identity;
                foodRb = other.GetComponent<Rigidbody2D>();
                foodRb.velocity = Vector2.zero;
                foodRb.bodyType = RigidbodyType2D.Kinematic;
                foodRb.freezeRotation = true;
                holdFood = true;
            }
        }
    }

    public void throwFood(Vector2 direction, bool forKing)
    {
        if(holdFood)
        {

            soundController.playSound(SoundController.Sound.LANCER);
            controller.setThrowing(true);
            GetComponent<PlayerAnimator>().changeAnimation(throwAnimation);
            food.transform.parent = null;
            holdFood = false;
            food.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            food.GetComponent<Rigidbody2D>().mass = massThrownObject;
            countTime = true;

            if (forKing)
            {
                food.GetComponent<FoodData>().setOwner(gameObject);
                food.layer = LayerMask.NameToLayer("FoodForKing");
                food.GetComponent<Rigidbody2D>().gravityScale = 0;
 
            }
            else
            {
                food.GetComponent<Rigidbody2D>().gravityScale = gravityThrownObject;
            }
            food.GetComponent<Rigidbody2D>().AddForce(direction * forceThrownObject, ForceMode2D.Impulse);

        }
    }
    public void looseFood()
    {
        Destroy(food);
        food = null;
        holdFood = false;
    }
    public void addScore(int amount)
    {
        score += amount;
        scoreTxt.text = score.ToString();
    }

    private void FixedUpdate()
    {
        
        if (countTime)
        {
            timer += Time.deltaTime;
            Vector2 direction = foodRb.velocity.x >= 0 ? Vector2.one : new Vector2(-1, 1);
            //if (receiver.FireLeft || receiver.FireRight) foodRb.AddForce(direction * throwPowerMultiplier, ForceMode2D.Impulse);
        }
        if (timer >= throwTime)
        {
            countTime = false;
            timer = 0;
            food = null;
            controller.setThrowing(false);
        }
    }
}
