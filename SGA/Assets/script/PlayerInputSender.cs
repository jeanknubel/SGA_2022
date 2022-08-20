using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]

public class PlayerInputSender : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInputReceiver playerInputReceiver;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += (Scene, mode) => LoadInputReceiver();

        LoadInputReceiver();
    }

    private void LoadInputReceiver()
    {
        PlayerInputReceiver[] receivers = FindObjectsOfType<PlayerInputReceiver>();
        
        //recherche le playerinput avec le bon index
        playerInputReceiver = receivers.FirstOrDefault(i => i.PlayerIndex == playerInput.playerIndex);

        if(receivers.Length < 1)
        {
            Debug.LogError("Not enough players.");
        }
    }

    private void OnMove(InputValue value)
    {
        playerInputReceiver.HorizontalAxis = value.Get<float>();
    }
    private void OnJump(InputValue value)
    {
        playerInputReceiver.Jump = value.Get<float>() > 0.5f;
    }
    private void OnFireRight(InputValue value)
    {
        playerInputReceiver.FireRight = value.Get<float>() > 0.5f;
    }
    private void OnFireLeft(InputValue value)
    {
        playerInputReceiver.FireLeft = value.Get<float>() > 0.5f;
    }
    private void OnAim(InputValue value)
    {
        playerInputReceiver.Aim = value.Get<Vector2>();
    }
    private void OnPause(InputValue value)
    {
        playerInputReceiver.Pause = value.Get<float>() > 0.5f;
    }
    private void OnSkip(InputValue value)
    {
        playerInputReceiver.Skip = value.Get<float>() > 0.5f;

    }
}
