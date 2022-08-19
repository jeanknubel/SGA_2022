using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingEat : MonoBehaviour
{
    private PlayerAnimator animator;
    [SerializeField]
    private SoundController soundController;
    private void Awake()
    {
        animator = GetComponent<PlayerAnimator>(); 
    }
    //king eat the food
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == LayerMask.NameToLayer("FoodForKing"))
        {
            FoodData data = other.GetComponent<FoodData>();
            data.getOwner().GetComponent<PlayerFoodInteraction>().addScore(data.getScore());
            Destroy(other);
            animator.changeAnimation("King_Eat");
            soundController.playSound(SoundController.Sound.EMPEREUR_MANGE);
            Invoke("returnIdle", 2);
        }
    }
    private void returnIdle()
    {
        animator.changeAnimation("King_Pending");
    }
}
