using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float distance;
    
    private PlayerInput playerInput;
    
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
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
        
    }
        
    private void MousePos(InputAction.CallbackContext obj)
    {
        Vector2 mouseScreenPosition = obj.ReadValue<Vector2>();
        
        // Debug: Log mouse screen position
        Debug.Log($"<color=yellow>[MOUSE] Screen Position: {mouseScreenPosition}</color>");
        
        // Create a ray from camera through mouse position
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
        
        Debug.Log($"<color=cyan>[RAY] Origin: {ray.origin}, Direction: {ray.direction}</color>");
        
        // Raycast to get world position
        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            Vector3 worldPosition = hit.point;
            
            Debug.Log($"<color=green>[HIT] World Position: {worldPosition}</color>");
            Debug.Log($"<color=green>[HIT] Hit Object: {hit.collider.gameObject.name}</color>");
            Debug.Log($"<color=green>[HIT] Distance from camera: {hit.distance}</color>");
        }
        else
        {
            // If no hit, use a default distance from camera
            Vector3 worldPosition = ray.origin + ray.direction * distance;
            
            Debug.Log($"<color=orange>[NO HIT] Using default world position at distance {distance}: {worldPosition}</color>");
        }
    }
    
}
