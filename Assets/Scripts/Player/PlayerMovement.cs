using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float movementSmoothFactor;
    
    [Header("Movement")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private float gravityScale;
    
    [Header("Camera")]
    [SerializeField] private float rotationSmoothFactor;
    
    [Header("Ground Detection")]
    [SerializeField] private Transform feet;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask whatIsGround;

    private CharacterController controller;

    private bool isGrounded;
    
    private Vector2 inputVector; 
    private Vector3 horizontalMovement; 
    private Vector3 verticalMovement;
    private Vector3 totalMovement;
    
    private PlayerInput input;
    
    private Animator anim;
    
    private float rotationVelocity;
    private float currentSpeed;
    private float targetSpeed;
    private float speedVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>(); 
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        input.actions["Move"].performed += UpdateMovement;
        input.actions["Move"].canceled += UpdateMovement;
    }
    private void OnDisable()
    {
        input.actions["Move"].performed -= UpdateMovement;
        input.actions["Move"].canceled -= UpdateMovement;
    }

    private void UpdateMovement(InputAction.CallbackContext ctx)
    {
        inputVector = ctx.ReadValue<Vector2>();
    }
    
    void Update()
    {
        GroundCheck(); 
        ApplyGravity();
        MoveAndRotate();
        
        if (totalMovement.magnitude > 0f)
        {
            anim.SetInteger("AnimationPar", 0);
        }
        
    }

    private void MoveAndRotate()
    {
        targetSpeed = movementSpeed * inputVector.magnitude;
        
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, movementSmoothFactor);
        
        if (inputVector.sqrMagnitude > 0)
        {
            float angleToRotate = Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg;
            
            
            horizontalMovement = (Quaternion.Euler(0, angleToRotate, 0) * Vector3.forward) * movementSpeed;

            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angleToRotate, ref rotationVelocity, rotationSmoothFactor);
           
            transform.rotation = Quaternion.Euler(0 ,smoothAngle, 0 );
        }
        else
        {
            horizontalMovement = Vector3.zero;
        }
        
        anim.SetInteger("AnimationPar", 1);
        
        totalMovement = horizontalMovement + verticalMovement;
       
        controller.Move(totalMovement * Time.deltaTime);
    }
    
    private void ApplyGravity()
    {
        if (isGrounded && verticalMovement.y < 0)
        {
            verticalMovement.y = -2f;
        }
        else
        {
            verticalMovement.y += gravityScale * Time.deltaTime;
        }
    }

    private void GroundCheck()
    {
        if (Physics.CheckSphere(feet.position, detectionRadius, whatIsGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(feet.position, detectionRadius);
    }
}
