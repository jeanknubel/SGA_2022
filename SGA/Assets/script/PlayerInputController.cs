using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private Transform ground_check;

    [SerializeField]
    private float hSpeed, jumpForce, velPowGround, velPowAir, accel, deccel, jumpReduce, gravityScale, gravityScaleMultiplier, wallFriction;

    [SerializeField]
    private float knockback;

    [SerializeField]
    private Vector2 wallJumpForce;

    [SerializeField]
    private Vector2 respawnPosition;

    [SerializeField]
    private Vector2 aimDirection, autoAimDirection;


    [SerializeField]
    private Transform upStage;

    [SerializeField]
    private bool canDoubleJump;
    [SerializeField]
    private float doubleJumpForce;
    [SerializeField]
    private float forceBuild;

    private Vector2 throwVector;
    private float hInpt;
    private float maxSpeed, speedDif, accelRate;
    private bool triggerJump, isGrounded, isOnWall, isJumping;
    private Rigidbody2D rb;
    private int groundLayer;
    private float playerDirection;
    private bool doubleJumped, releasedJump;
    private PlayerInputReceiver receiver;
    private bool fireRight, fireLeft, isOnToriLeft, isOnToriRight, lastFireRight, lastFireLeft;
    private PlayerAnimator animator;
    private PlayerFoodInteraction interactor;
    private bool gameStarted;
    private bool isThrowing;
    private bool oldJumpTrigger;
    private bool jumped;
    private bool hasFallen;
    [SerializeField]
    private SoundController soundController;
    private float throwForce, throwTime ;
    [SerializeField]
    private float initialThrowForce, maxThrowTime;

    [SerializeField]
    private string animationPending, animationFall, animationJump, animationRun, animationRunFruit, animationThrow, animationWallJump, animationPendingFood, animationJumpFront;

    public void startGame()
    {
        gameStarted = true;
        isThrowing = false;
        hasFallen = false;
        respawn();
        jumped = false;
    }
    public void setThrowing(bool value) { isThrowing = value; }
    private void Awake()
    {
        receiver = GetComponent<PlayerInputReceiver>();
        print(receiver);
        gameStarted = false;
        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        animator = GetComponent<PlayerAnimator>();
        interactor = GetComponent<PlayerFoodInteraction>();
        groundLayer = LayerMask.GetMask("Ground", "Food");
        playerDirection = 1;
        doubleJumped = false;
        releasedJump = false;
    }

    void Update()
    {
        if (gameStarted)
        {
            //movement avec acceleration et jump noraml-----------------------------------------
            hInpt = receiver.HorizontalAxis;
            triggerJump = receiver.Jump;
            lastFireRight = fireRight;
            lastFireLeft = fireLeft;
            fireRight = receiver.FireRight;
            fireLeft = receiver.FireLeft;
            //----------------------------------------------------------------------------
        }

    }
    private void FixedUpdate()
    {
        if (fireRight) print("fire");
        bool wantJump = triggerJump && !oldJumpTrigger;
        //animation handling
        if (!isThrowing)
        {
            if (isOnWall) animator.changeAnimation(animationWallJump);
            else if (rb.velocity.x == 0 && isGrounded && hInpt == 0)
            {
                if (interactor.isHoldingFood()) animator.changeAnimation(animationPendingFood);
                else animator.changeAnimation(animationPending);
            }
            else if ((hInpt != 0 || rb.velocity.x != 0) && isGrounded)
            {
                if (interactor.isHoldingFood()) animator.changeAnimation(animationRunFruit);
                else animator.changeAnimation(animationRun);
            }
            else if (!isGrounded && rb.velocity.y < 0 && rb.velocity.x != 0) animator.changeAnimation(animationFall);
            else if (!isGrounded && rb.velocity.y != 0 && rb.velocity.x == 0 && hInpt == 0) animator.changeAnimation(animationJumpFront);
            else if (!isGrounded && rb.velocity.y > 0) animator.changeAnimation(animationJump);
        }


        //wall jump si on appuie dans la direction opposée de ou on vient
        if (isOnWall && hInpt > 0 && Mathf.Abs(rb.velocity.x) <= 0.00000001)
        {
            soundController.playSound(SoundController.Sound.JUMP);
            rb.AddForce(new Vector2(wallJumpForce.x * -playerDirection, wallJumpForce.y), ForceMode2D.Impulse);
        }

        //changement de direction
        transform.localScale = new Vector3(hInpt > 0 ? 1: hInpt < 0 ? -1:  transform.localScale.x,  1, 1);



        isOnToriRight = rb.position.x > 4 && isGrounded;
        isOnToriLeft = rb.position.x < -4 && isGrounded;

        isGrounded = Physics2D.OverlapBox(ground_check.position, new Vector2(0.4f, 0.5f), 0, groundLayer);
        isOnWall = !isGrounded && Physics2D.OverlapBox(rb.transform.position, new Vector2(1.1f, 0.2f), 0, LayerMask.GetMask("Wall")) ;

        if (isGrounded)
        {
            doubleJumped = false;
            releasedJump = false;
        }
        //movement avec acceleration -------------------------------------------------
        maxSpeed = hInpt * hSpeed;
        speedDif = maxSpeed - rb.velocity.x;
        accelRate = Mathf.Abs(maxSpeed) > 0.01f ? accel : deccel;
        float velPow = isGrounded ? velPowGround : velPowAir;
        rb.AddForce(Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPow) * Mathf.Sign(speedDif) * Vector2.right);
        //----------------------------------------------------------------------------

        if (rb.velocity.y != 0 && !triggerJump) releasedJump = true;
        if (rb.velocity.y < 0) isJumping = false;



        playerDirection = rb.velocity.x > 0 ? 1 : rb.velocity.x < 0 ? -1 : playerDirection;

        //reset la vitesse y si on arrive sur le mur pour qu'on glisse tjr à la même vitesse
        //if (isOnWall && rb.velocity.x != 0)
          //  rb.velocity = new Vector2(rb.velocity.x, 0);
        //friction sur les murs
        if (isOnWall && rb.velocity.y < 0)
            rb.AddForce(Vector2.up * wallFriction * Time.deltaTime);

        //permet de sauter plus ou moins haut selon comment on appuie sur le bouton
        if (rb.velocity.y > 0 && !triggerJump && isJumping)
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpReduce), ForceMode2D.Impulse);

        //plus de gravité quand on tombe
        if (rb.velocity.y < 0) rb.gravityScale = gravityScale * gravityScaleMultiplier;
        else rb.gravityScale = gravityScale;

        //saut normal
        if (triggerJump && isGrounded && !isJumping)
        {
            jumped = true;
            Invoke("resetJump", 0.5f);
            isJumping = true;
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            soundController.playSound(SoundController.Sound.JUMP);
        }

        //double jump
        if (rb.velocity.y != 0 && !isGrounded && !doubleJumped && canDoubleJump && triggerJump && !isOnWall && releasedJump)
        {
            isJumping = true;
            doubleJumped = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, doubleJumpForce), ForceMode2D.Impulse);
            soundController.playSound(SoundController.Sound.JUMP);

        }

        if (fireRight && !lastFireRight && !isOnToriLeft)
        {
            throwVector = receiver.Aim;
            if (throwVector == Vector2.zero)
                throwVector = aimDirection;
            throwTime = 0;
            //GetComponent<PlayerFoodInteraction>().throwFood(aimDirection, false);

        }
        if (fireLeft && !isOnToriRight && !lastFireLeft)
        {
            throwVector = receiver.Aim;
            if (throwVector == Vector2.zero)
                throwVector = new Vector2(-aimDirection.x, aimDirection.y);
            //GetComponent<PlayerFoodInteraction>().throwFood(new Vector2(-aimDirection.x, aimDirection.y), false);
            throwTime = 0;

        }
        if (((fireLeft && lastFireLeft) || (fireRight && lastFireRight)) && throwTime <= maxThrowTime)
        {
            throwForce += forceBuild;
            throwTime += Time.deltaTime;
            print(throwForce);
        }
        if(!fireRight && lastFireRight)
        {
            print("fire");
        }
        if ((fireRight || fireLeft) && isOnToriLeft){
            transform.localScale = new Vector3(1, 1, 1);
            GetComponent<PlayerFoodInteraction>().throwFood(autoAimDirection, true);

        }
        if ((fireRight || fireLeft) && isOnToriRight) {
            GetComponent<PlayerFoodInteraction>().throwFood(new Vector2(-autoAimDirection.x, autoAimDirection.y), true);
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (gameObject.transform.position.y <= -4.2 && !hasFallen)
        {
            soundController.playSound(SoundController.Sound.CHUTE);
            hasFallen = true;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.layer == LayerMask.NameToLayer("Player"))
        {
            Vector2 ejection = collision.GetContact(0).normal;
            if (ejection.y != 0) ejection.y /= 2;

            rb.AddForce(ejection * knockback, ForceMode2D.Impulse);
        }
    }

    public void respawn()
    {
        rb.velocity = Vector2.zero;
        rb.transform.position = respawnPosition;
        GetComponent<PlayerFoodInteraction>().looseFood();
        doubleJumped = false;
        hasFallen = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(ground_check.position, new Vector2(0.4f, 0.5f));
        Gizmos.DrawWireCube(transform.position, new Vector2(1.1f, 0.2f));
    }

    private void resetJump()
    {
        jumped = false;
    }
}
