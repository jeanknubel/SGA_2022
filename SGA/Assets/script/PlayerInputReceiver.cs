using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputReceiver : MonoBehaviour
{
    [SerializeField]
    private int playerIdx;

    public float HorizontalAxis { get; set; }
    public bool Jump { get; set; }
    public bool FireRight { get; set; }
    public bool FireLeft { get; set; }
    public Vector2 Aim { get; set; }

    public int PlayerIndex => playerIdx;
}
