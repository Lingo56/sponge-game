using UnityEngine;
using UnityEngine.InputSystem;

// TODO: BUG - Player can climb capsule collider walls.
[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float resetHeight = -1f;
    public Vector3 spawnPoint;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 movementInput;
    private InputAction moveAction;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        moveAction = playerControls.Player.Move;
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
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