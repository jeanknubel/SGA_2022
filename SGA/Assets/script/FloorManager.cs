using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == LayerMask.NameToLayer("Player")) other.GetComponent<PlayerInputController>().respawn();
        if (other.layer == LayerMask.NameToLayer("Food")) Destroy(other);
    }
}
