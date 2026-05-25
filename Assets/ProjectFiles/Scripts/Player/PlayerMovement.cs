using System.Collections;
using ProjectFiles.Scripts.Cubes._2x2Cube;
using ProjectFiles.Scripts.Cubes._3x3Cube;
using ProjectFiles.Scripts.Cubes._4x4Cube;
using ProjectFiles.Scripts.Game_Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


namespace ProjectFiles.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float movementSmoothFactor;

        [Header("Movement")] [SerializeField] private float movementSpeed;
        [SerializeField] private float gravityScale;
        [SerializeField] private Transform hip;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float raycastDistance;
        private Vector3 gravityDirection = Vector3.down;
        

        [Header("Camera")] [SerializeField] 
        private float rotationSmoothFactor;

        [Header("Ground Detection")] [SerializeField]
        private Transform feet;

        [Header("Sound Effects")] [SerializeField]
        private AudioClip changeFaceCharacterSFX;
        [SerializeField] private AudioClip playerSteps1;
        [SerializeField] private AudioClip playerSteps2;
        
        [Header("GroundDetection")]
        [SerializeField] private float detectionRadius;
        [SerializeField] private LayerMask whatIsGround;

        private CharacterController controller;

        private bool isGrounded;
       

        private Vector2 inputVector;
        private Vector3 horizontalMovement;
        private Vector3 verticalMovement;
        private Vector3 verticalVelocity;
        private Vector3 totalMovement;
        private Vector3 previousGravityDirection = Vector3.down;

        private PlayerInput input;

        [SerializeField] Animator anim;

        private float rotationVelocity;
        private float currentSpeed;
        private float targetSpeed;
        private float speedVelocity;
        
        
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        private LayerMask cubeFaceLayer;

        private bool isWalking;
        private bool stepsCoroutineRunning;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<PlayerInput>();

            if (mainCamera == null)
                mainCamera = Camera.main;
            
            initialPosition = transform.position;
            initialRotation = transform.rotation;

            cubeFaceLayer = LayerMask.GetMask("Faces");
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
            
            if (gravityDirection != previousGravityDirection)
            {
                verticalVelocity = gravityDirection.normalized * 0.1f; 
                previousGravityDirection = gravityDirection;
                Debug.Log($"Cambio de cara detectado: {previousGravityDirection} → {gravityDirection}");
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetPosition();
                SC_SFXManager.Instance.PlaySoundFXClip(changeFaceCharacterSFX, transform, 1f);
            }

            Debug.Log(totalMovement.magnitude);

            if (isWalking && isGrounded && !stepsCoroutineRunning)
            {
                StartCoroutine(PlayerSteps());
            }
            else if (!isWalking && stepsCoroutineRunning)
            {
                StopCoroutine(PlayerSteps());
                stepsCoroutineRunning = false;
            }

            if (PauseMenu.isPaused)
            {
                controller.enabled = false;
                input.enabled = false;
            }
            else
            {
                controller.enabled = true;
                input.enabled = true;
            }
            
            if (AutoShuffle.is3x3Shuffling)
            {
                controller.enabled = false;
                input.enabled = false;
            }
            else
            {
                controller.enabled = true;
                input.enabled = true;
            }
            
            if (AutoShuffle2x2.is2x2ShuffleActive)
            {
                controller.enabled = false;
                input.enabled = false;
            }
            else
            {
                controller.enabled = true;
                input.enabled = true;
            }
            
            if (AutoShuffle4x4.is4x4ShuffleActive)
            {
                controller.enabled = false;
                input.enabled = false;
            }
            else
            {
                controller.enabled = true;
                input.enabled = true;
            }
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
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
                        Time.deltaTime / rotationSmoothFactor);
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


            if (inputVector.sqrMagnitude > 0)
            {
                anim.SetInteger("AnimationPar", 1);
                isWalking = true;

            }
            else
            {
                anim.SetInteger("AnimationPar",0);
                isWalking = false;
            }

            totalMovement = horizontalMovement + verticalMovement;
            controller.Move(totalMovement * Time.deltaTime);
        }

        private Vector3 AdaptVectorToGravity(Vector3 vector)
        {
            return Vector3.ProjectOnPlane(vector, gravityDirection).normalized;
        }


        private void ApplyGravity()
        {
            if (isGrounded)
            {
                verticalVelocity = gravityDirection.normalized * 0.1f;
            }
            else
            {
                verticalVelocity += gravityDirection.normalized * (gravityScale * Time.deltaTime);
            }

            verticalMovement = verticalVelocity;
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
        
        private void ResetPosition()
        {
            controller.enabled = false;
            
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            
            controller.enabled = true;
            
            verticalVelocity = Vector3.zero;
            horizontalMovement = Vector3.zero;
            verticalMovement = Vector3.zero;
            currentSpeed = 0f;
            targetSpeed = 0f;
            gravityDirection = Vector3.down;

            Debug.Log("Posición reiniciada a la inicial");
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
            Physics.Raycast(hip.position, Vector3.right, out RaycastHit rightRay, raycastDistance,cubeFaceLayer);
            if (rightRay.collider != null)
            {
                gravityDirection = Vector3.right;
            }

            Physics.Raycast(hip.position, Vector3.left, out RaycastHit leftRay, raycastDistance,cubeFaceLayer);
            if (leftRay.collider != null)
            {
                gravityDirection = Vector3.left;
            }

            Physics.Raycast(hip.position, Vector3.forward, out RaycastHit forwardRay, raycastDistance,cubeFaceLayer);
            if (forwardRay.collider != null)
            {
                gravityDirection = Vector3.forward;
            }

            Physics.Raycast(hip.position, -Vector3.forward, out RaycastHit backwardsRay, raycastDistance,cubeFaceLayer);
            if (backwardsRay.collider != null)
            {
                gravityDirection = -Vector3.forward;
            }

            Physics.Raycast(hip.position, Vector3.down, out RaycastHit downRay, raycastDistance,cubeFaceLayer);
            if (downRay.collider != null)
            {
                gravityDirection = Vector3.down;
            }
            
        }

        private IEnumerator PlayerSteps()
        {
            stepsCoroutineRunning = true;
    
            while (isWalking && isGrounded) 
            {
                SC_SFXManager.Instance.PlaySoundFXClip(playerSteps1, transform, 1f);
                yield return new WaitForSeconds(0.5f);
        
                if (!isWalking || !isGrounded) break; 
        
                SC_SFXManager.Instance.PlaySoundFXClip(playerSteps2, transform, 1f);
                yield return new WaitForSeconds(0.5f);
            }
    
            stepsCoroutineRunning = false;
        }
        
    }
}
