using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingEat : MonoBehaviour
{
    private PlayerAnimator animator;
    [SerializeField]
    private SoundController soundController;
    [SerializeField]
    private GameObject feu;
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
            if (data.getScore() > 0)
            {
                animator.changeAnimation("King_Eat");
                soundController.playSoundFx(SoundController.Sound.FX_EMPEREUR);
                soundController.playSound(SoundController.Sound.EMPEREUR_MANGE);
            }
            else
            {
                animator.changeAnimation("King_Colere");
                feu.SetActive(true);
                //feu.GetComponent<SpriteRenderer>().enabled = true;
                feu.GetComponent<Animator>().Play("Feu");
                soundController.playSoundFx(SoundController.Sound.EMPEREUR_GROGNEMENT);

            }
            Invoke("returnIdle", 2);
        }
    }
    private void returnIdle()
    {
        feu.SetActive(false);
        animator.changeAnimation("King_Pending");
    }
}
