using UnityEngine;
using UnityEngine.InputSystem;

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
        movementInput = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.y;

        // Move the character
        controller.Move(move * (speed * Time.deltaTime));

        // Apply gravity
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0f;
        }

        controller.Move(velocity * Time.deltaTime);

        if (gameObject.transform.position.y < resetHeight)
        {
            Vector3 offset = spawnPoint - transform.position;
            controller.Move(offset);
        }
    }
}