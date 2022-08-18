using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private string currentAnimationName;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void changeAnimation(string animationName)
    {
        if(animationName != currentAnimationName)
        {
            animator.Play(animationName);
            currentAnimationName = animationName;
        }
    }

}
