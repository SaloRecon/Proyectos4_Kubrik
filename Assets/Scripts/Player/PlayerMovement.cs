using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] private float movementSpeed;
    
    
    [SerializeField] private float sampleDistance;
    
    private NavMeshAgent player;
    private PlayerInput playerInput;
    private Animator playerAnim;
    [SerializeField] private LayerMask groundLayer;

    private Vector2 mousePosition;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<NavMeshAgent>();
        playerAnim = GetComponent<Animator>();

        player.speed = movementSpeed;
    }
    
    private void OnEnable()
    {
        playerInput.actions["Movement"].started += Movement;
        playerInput.actions["MousePos"].performed += MousePos;
    }


    private void OnDisable()
    {
        playerInput.actions["Movement"].started -= Movement;
        playerInput.actions["MousePos"].performed -= MousePos;
    }
    
    private void Movement(InputAction.CallbackContext obj)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, groundLayer))
        {
            if(NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, sampleDistance, NavMesh.AllAreas))
            {
                player.SetDestination(navHit.position);
            }
            else
            {
                Debug.Log("No valid NavMesh position found near the clicked point.");
            }
        }
        
        
    }
        
    private void MousePos(InputAction.CallbackContext obj)
    {
        mousePosition = obj.ReadValue<Vector2>();
    }
    
}
