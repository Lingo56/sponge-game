using Events;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float resetHeight = -1f;
    [SerializeField] private Vector3 spawnPoint;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 movementInput;
    private InputAction moveAction;
    private PlayerControls playerControls;

    // Add moveEnabled property
    private bool moveEnabled = true;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        GameEvents.OnEnablePlayerMovement += EnablePlayerMovement;
        GameEvents.OnDisablePlayerMovement += DisablePlayerMovement;
        moveAction = playerControls.Player.Move;
        moveAction.Enable();
    }

    private void OnDisable()
    {
        GameEvents.OnEnablePlayerMovement -= EnablePlayerMovement;
        GameEvents.OnDisablePlayerMovement -= DisablePlayerMovement;
        moveAction.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void EnablePlayerMovement()
    {
        moveEnabled = true;
    }
    
    private void DisablePlayerMovement()
    {
        moveEnabled = false;
    }
    
    void Update()
    {
        if (!moveEnabled) return; // Disable movement if moveEnabled is false

        // Get input
        movementInput = moveAction.ReadValue<Vector2>();
    
        // Create movement vector from input
        Vector3 moveDirection = transform.right * movementInput.x + transform.forward * movementInput.y;
        moveDirection.Normalize(); // Normalize to prevent faster diagonal movement
    
        // Apply speed
        Vector3 movementVector = moveDirection * speed;
    
        // Apply gravity - combine with horizontal movement
        if (controller.isGrounded)
        {
            velocity.y = -0.5f; // Small constant downward force when grounded
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    
        // Combine horizontal movement with vertical velocity
        movementVector.y = velocity.y;
    
        // Apply all movement in one controller.Move call
        controller.Move(movementVector * Time.deltaTime);
    
        // Reset position if below threshold
        if (transform.position.y < resetHeight)
        {
            Vector3 offset = spawnPoint - transform.position;
            controller.Move(offset);
        }
    }
}