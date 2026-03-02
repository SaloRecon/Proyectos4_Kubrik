using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController playerController;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerController = GetComponent<CharacterController>();
    }
    
    private void OnEnable()
    {
            playerInput.actions["Movement"].started += Movement;
    }
    
    private void OnDisable()
    {
        playerInput.actions["Movement"].started -= Movement;
    }
    
    private void Movement(InputAction.CallbackContext obj)
    {
        
    }
        
}
