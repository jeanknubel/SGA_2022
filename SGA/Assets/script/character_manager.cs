using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//gère les déplacements des 2 joueurs
public class character_manager : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D player1_RB, player2_RB;
    [SerializeField]
    private Transform ground_check_1, ground_check_2;

    [SerializeField]
    private float speed;




    void Update()
    {

        float horizontal1 = Input.GetKey("a") ? -1 : Input.GetKey("d") ? 1 : 0;
        float horizontal2 = Input.GetKey("left") ? -1 : Input.GetKey("right") ? 1 : 0;


        player1_RB.velocity = new Vector2(horizontal1 * speed, player1_RB.velocity.y);
        player2_RB.velocity = new Vector2(horizontal2 * speed, player2_RB.velocity.y);
    }
    void FixedUpdate()
    {
    }
}
