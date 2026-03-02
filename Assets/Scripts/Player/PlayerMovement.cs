using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
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
