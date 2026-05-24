using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSmoothFactor;
    
    [Header("Movement")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private float gravityScale;
    [SerializeField] private Transform hip;
    [SerializeField] private Camera mainCamera; 
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
        
        if (mainCamera == null)
            mainCamera = Camera.main;
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
            
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            
            
            cameraForward = AdaptVectorToGravity(cameraForward);
            cameraRight = AdaptVectorToGravity(cameraRight);
            
            
            Vector3 moveDirection = (cameraRight * inputVector.x + cameraForward * inputVector.y).normalized;
            
            
            moveDirection = Vector3.ProjectOnPlane(moveDirection, gravityDirection).normalized;
            
            if (moveDirection.magnitude > 0.01f)
            {
                horizontalMovement = moveDirection * movementSpeed;
                
                
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, -gravityDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / rotationSmoothFactor);
            }
            else
            {
                horizontalMovement = Vector3.zero;
            }
        }
        else
        {
            horizontalMovement = Vector3.zero;
        }
        
        anim.SetInteger("AnimationPar", inputVector.sqrMagnitude > 0 ? 1 : 0);
        
        totalMovement = horizontalMovement + verticalMovement;
        controller.Move(totalMovement * Time.deltaTime);
    }
    
    private Vector3 AdaptVectorToGravity(Vector3 vector)
    {
        return Vector3.ProjectOnPlane(vector, gravityDirection).normalized;
    }
    
    private void ApplyGravity()
    {
        verticalMovement = gravityDirection * Time.deltaTime * gravityScale;
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(feet.position, gravityDirection, detectionRadius, whatIsGround))
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
        if (feet != null)
        {
            Gizmos.DrawSphere(feet.position, detectionRadius);
        }
        
        if (hip != null)
        {
            Gizmos.DrawRay(hip.position, Vector3.right);
            Gizmos.DrawRay(hip.position, Vector3.left);
            Gizmos.DrawRay(hip.position, Vector3.down);
            Gizmos.DrawRay(hip.position, Vector3.forward);
            Gizmos.DrawRay(hip.position, -Vector3.forward);
        }
    }
    
    private void HipRaycast()
    {
        Physics.Raycast(hip.position, Vector3.right, out RaycastHit rightRay, 5f);
        if(rightRay.collider != null)
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
