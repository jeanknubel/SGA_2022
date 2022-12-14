using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private LevelManager manager;

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
    private GameObject throwEffect;

    [SerializeField]
    private bool canDoubleJump;
    [SerializeField]
    private float doubleJumpForce;
    [SerializeField]
    private float forceBuild;

    [SerializeField]
    private GameObject healthbar;

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
    private bool roundPlaying;
    private bool isThrowing;
    private bool oldJumpTrigger;
    private bool jumped;
    private bool hasFallen;
    private bool isSelectingDaruma;

    [SerializeField]
    private SoundController soundController;
    private float throwForce;
    [SerializeField]
    private float initialThrowForce, maxThrowForce;
    private bool mustFire;
    private bool fireRightDown, fireLeftDown;

    [SerializeField]
    private string animationPending, animationFall, animationJump, animationRun, animationRunFruit, animationThrow, animationWallJump, animationPendingFood, animationJumpFront;

    public void startGame()
    {
        roundPlaying = true;
        isSelectingDaruma = false;
        isThrowing = false;
        hasFallen = false;
        respawn();
        jumped = false;
        mustFire = false;
        healthbar.GetComponent<Slider>().maxValue = maxThrowForce;
        healthbar.GetComponent<Slider>().minValue = initialThrowForce;
        healthbar.GetComponent<Slider>().value = initialThrowForce;

        healthbar.SetActive(false);
        throwForce = initialThrowForce;

    }
    public void selectDaruma()
    {
        isSelectingDaruma = true;
    }
    public void endRound()
    {
        roundPlaying = false;
        respawn();
    }
    public void setThrowing(bool value) { isThrowing = value; }
    private void Awake()
    {
        receiver = GetComponent<PlayerInputReceiver>();
        print(receiver);
        roundPlaying = false;
        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        throwEffect.SetActive(false);
        animator = GetComponent<PlayerAnimator>();
        interactor = GetComponent<PlayerFoodInteraction>();
        groundLayer = LayerMask.GetMask("Ground", "Food");
        playerDirection = 1;
        doubleJumped = false;
        releasedJump = false;
    }

    void Update()
    {
        if (manager.isVideoPlaying())
        {
            if (receiver.Skip) manager.stopVideo();
        }
        if(roundPlaying || isSelectingDaruma)
        {
            triggerJump = receiver.Jump;
        }
        if (roundPlaying && !isSelectingDaruma)
        {
            //movement avec acceleration et jump noraml-----------------------------------------
            hInpt = receiver.HorizontalAxis;
            lastFireRight = fireRight;
            lastFireLeft = fireLeft;
            fireRight = receiver.FireRight;
            fireLeft = receiver.FireLeft;
            if ((!fireRight && lastFireRight) || (!fireLeft && lastFireLeft)) mustFire = true;
            if (fireRight && !lastFireRight) fireRightDown = true;
            if (fireLeft && !lastFireLeft) fireLeftDown = true;
            if (receiver.Pause) manager.pauseGame();
            //----------------------------------------------------------------------------
        }

    }
    private void FixedUpdate()
    {
        bool wantJump = triggerJump && !oldJumpTrigger;
        //animation handling
        if (!isThrowing)
        {
            if (isOnWall) animator.changeAnimation(animationWallJump);
            else if (Mathf.Abs(rb.velocity.x) <= 0.000000001 && isGrounded && hInpt == 0)
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


        //wall jump si on appuie dans la direction oppos?e de ou on vient
        if (isOnWall && hInpt == -playerDirection && Mathf.Abs(rb.velocity.x) <= 0.00000001)
        {
            soundController.playSound(SoundController.Sound.JUMP);
            rb.AddForce(new Vector2(wallJumpForce.x * -playerDirection, wallJumpForce.y), ForceMode2D.Impulse);
        }

        //changement de direction TODO: bug sur les tori
        transform.localScale = new Vector3(hInpt > 0 ? 1: hInpt < 0 ? -1: fireRight ? 1 : fireLeft ? -1 : transform.localScale.x,  1, 1);



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

        //reset la vitesse y si on arrive sur le mur pour qu'on glisse tjr ? la m?me vitesse
        //if (isOnWall && rb.velocity.x != 0)
          //  rb.velocity = new Vector2(rb.velocity.x, 0);
        //friction sur les murs
        if (isOnWall && rb.velocity.y < 0)
            rb.AddForce(Vector2.up * wallFriction * Time.deltaTime);

        //permet de sauter plus ou moins haut selon comment on appuie sur le bouton
        if (rb.velocity.y > 0 && !triggerJump && isJumping)
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpReduce), ForceMode2D.Impulse);

        //plus de gravit? quand on tombe
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

        if (fireRightDown && !isOnToriLeft && interactor.isHoldingFood())
        {
            healthbar.SetActive(true);
            throwVector = receiver.Aim;
            fireRightDown = false;
            if (throwVector == Vector2.zero)
                throwVector = aimDirection;
            //GetComponent<PlayerFoodInteraction>().throwFood(aimDirection, false);

        }
        if (fireLeftDown && !isOnToriRight && interactor.isHoldingFood())
        {
            healthbar.SetActive(true);
            throwVector = receiver.Aim;
            fireLeftDown = false;
            if (throwVector == Vector2.zero)
                throwVector = new Vector2(-aimDirection.x, aimDirection.y);
            //GetComponent<PlayerFoodInteraction>().throwFood(new Vector2(-aimDirection.x, aimDirection.y), false);

        }
        if (((fireLeft && lastFireLeft) || (fireRight && lastFireRight)) && throwForce <= maxThrowForce && interactor.isHoldingFood())
        {
            throwForce += forceBuild * Time.deltaTime;
            healthbar.GetComponent<Slider>().value = throwForce;
        }
        if(mustFire && interactor.isHoldingFood() && !isOnToriLeft && !isOnToriRight)
        {
            throwEffect.SetActive(true);
            var animator = throwEffect.GetComponent<Animator>();
            animator.Play("Throw", -1, 0f);
            GetComponent<PlayerFoodInteraction>().throwFood(throwVector * throwForce, false);
            throwForce = initialThrowForce;
            mustFire = false;
            healthbar.SetActive(false);
            healthbar.GetComponent<Slider>().value = initialThrowForce;
        }
        if (fireRight && isOnToriLeft && interactor.isHoldingFood()){
            transform.localScale = new Vector3(1, 1, 1);
            GetComponent<PlayerFoodInteraction>().throwFood(autoAimDirection, true);
            mustFire = false;
            healthbar.SetActive(false);
            fireRightDown = false;

        }
        if ( fireLeft && isOnToriRight && interactor.isHoldingFood()) {
            GetComponent<PlayerFoodInteraction>().throwFood(new Vector2(-autoAimDirection.x, autoAimDirection.y), true);
            transform.localScale = new Vector3(1, 1, 1);
            mustFire = false;
            healthbar.SetActive(false);
            fireLeftDown = false;

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
            soundController.playSound(SoundController.Sound.COLLISION);
            Vector2 ejection = collision.GetContact(0).normal;
            if (ejection.y != 0) ejection.y /= 2;

            rb.AddForce(ejection * knockback, ForceMode2D.Impulse);
        }
        if (other.layer == LayerMask.NameToLayer("ThrownFood"))
        {
            int score = other.gameObject.GetComponent<FoodData>().getScore();
            if (score < 0)
            {
                Vector2 ejection = collision.GetContact(0).normal;
                ejection.y = 0.3f;
                ejection.x = ejection.x > 0 ? -1 : 1;
                print(ejection);
                GetComponent<PlayerFoodInteraction>().addScore(score);
                rb.AddForce(ejection * knockback * 2 , ForceMode2D.Impulse);
            }
            //other.layer = LayerMask.NameToLayer("Food");
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
