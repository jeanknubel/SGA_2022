using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    [SerializeField]
    private Transform ground_check;

    [SerializeField]
    private float hSpeed, jumpForce, velPowGround, velPowAir, accel, deccel, jumpReduce, gravityScale, gravityScaleMultiplier, wallFriction;

    [SerializeField]
    private Vector2 wallJumpForce;

    [SerializeField]
    private string left, right, jump;

    [SerializeField]
    private Vector2 respawnPosition;

    private float hInpt;
    private float maxSpeed, speedDif, accelRate;
    private bool wantJump, triggerJump, isGrounded, isOnWall;
    private Rigidbody2D rb;
    private int groundLayer;
    private float playerDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground");
        playerDirection = 1;
    }
    void Update()
    {
        //reset pour si on tombe
        if (Input.GetKeyDown("r"))
        {
            rb.transform.position = respawnPosition;
            rb.velocity = Vector2.zero;

        }

        //movement avec acceleration et jump noraml-----------------------------------------
        hInpt = Input.GetKey(left) ? -1 : Input.GetKey(right) ? 1 : 0;
        wantJump = Input.GetKey(jump);
        triggerJump = Input.GetKeyDown(jump);
        if (triggerJump && isGrounded)
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        //----------------------------------------------------------------------------

    }
    private void FixedUpdate()
    {
        //movement avec acceleration -------------------------------------------------
        maxSpeed = hInpt * hSpeed;
        speedDif = maxSpeed - rb.velocity.x;
        accelRate = Mathf.Abs(maxSpeed) > 0.01f ? accel : deccel;
        if (!isOnWall)
        {
            float velPow = isGrounded ? velPowGround : velPowAir;
            rb.AddForce(Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPow) * Mathf.Sign(speedDif) * Vector2.right);
        }
        //----------------------------------------------------------------------------


        isGrounded = Physics2D.OverlapBox(ground_check.position, new Vector2(0.2f, 0.2f), 0, groundLayer);
        isOnWall = Physics2D.OverlapBox(rb.transform.position, new Vector2(0.5f, 0.2f), 0, LayerMask.GetMask("Wall"));
        playerDirection = rb.velocity.x > 0 ? 1 : rb.velocity.x < 0 ? -1 : playerDirection;

        //reset la vitesse y si on arrive sur le mur pour qu'on glisse tjr à la même vitesse
        if (isOnWall && rb.velocity.x != 0)
            rb.velocity = new Vector2(rb.velocity.x, 0);
        //friction sur les murs
        if (isOnWall && rb.velocity.y < 0)
            rb.AddForce(Vector2.up * wallFriction);
        //wall jump si on appuie dans la direction opposée de ou on vient
        if (isOnWall && hInpt == -playerDirection)
            rb.AddForce(new Vector2(wallJumpForce.x * Mathf.Sign(hInpt), wallJumpForce.y), ForceMode2D.Impulse);

        //permet de sauter plus ou moins haut selon comment on appuie sur le bouton
        if (rb.velocity.y > 0 && !wantJump)
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpReduce), ForceMode2D.Impulse);

        //plus de gravité quand on tombe
        if (rb.velocity.y < 0) rb.gravityScale = gravityScale * gravityScaleMultiplier;
        else rb.gravityScale = gravityScale;
    }
}
