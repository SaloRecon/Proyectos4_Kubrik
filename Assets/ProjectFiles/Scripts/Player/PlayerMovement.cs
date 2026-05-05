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
    [SerializeField] private Transform hip;
    private Vector3 gravityDirection = Vector3.down;
    
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
    
    [SerializeField] Animator anim;
    
    private float rotationVelocity;
    private float currentSpeed;
    private float targetSpeed;
    private float speedVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>(); 
        
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
        Debug.Log("Input Vector: " + inputVector);
    }
    
    void Update()
    {
        GroundCheck(); 
        MoveAndRotate();
        HipRaycast();
        ApplyGravity();
        
        if (totalMovement.magnitude > 0f)
        {
            anim.SetInteger("AnimationPar", 1);
        }
        
        Debug.Log(totalMovement.magnitude);
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
        
        
        anim.SetInteger("AnimationPar", 0);
        
        totalMovement = horizontalMovement + verticalMovement;
       
        controller.Move(totalMovement * Time.deltaTime);
    }
    
    private void ApplyGravity()
    {
        verticalMovement = gravityDirection * Time.deltaTime * gravityScale;
       
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(feet.position, Vector3.down, detectionRadius, whatIsGround))
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
        
        Gizmos.DrawRay(hip.position, Vector3.right);
        Gizmos.DrawRay(hip.position, Vector3.left);
        Gizmos.DrawRay(hip.position, Vector3.down);
        Gizmos.DrawRay(hip.position, Vector3.forward);
        Gizmos.DrawRay(hip.position, -Vector3.forward);
        
        
    }
    
    
    private void HipRaycast()
    {
        Physics.Raycast(hip.position, Vector3.right, out RaycastHit rightRay, 5f);
        if(rightRay.collider  != null)
        {
            gravityDirection = Vector3.right;
        }
        
        Physics.Raycast(hip.position, Vector3.left, out RaycastHit leftRay, 5f);
        if (leftRay.collider != null)
        {
           gravityDirection = Vector3.left;
            
        }
        
        Physics.Raycast(hip.position, Vector3.forward, out RaycastHit forwardRay, 5f);
        if(forwardRay.collider != null)
        {
            gravityDirection = Vector3.forward;
        }
        
        Physics.Raycast(hip.position, -Vector3.forward, out RaycastHit backwardsRay, 5f);
        if(backwardsRay.collider != null)
        {
            gravityDirection = -Vector3.forward;
        }
        
        Physics.Raycast(hip.position, Vector3.down, out RaycastHit downRay, 5f);
        if(downRay.collider != null)
        {
            gravityDirection = Vector3.down;
        }
        
        
    }
}
