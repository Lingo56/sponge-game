using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private InputAction interactAction;

    public static event System.Action OnInteract; // Generic interact event
    public static event System.Action OnToggleMouseLook;

    private void Awake()
    {
        interactAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/e");
        interactAction.Enable();
    }

    private void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            OnInteract?.Invoke(); // Trigger the generic interact event
        }
    }

    private void OnDestroy()
    {
        interactAction.Disable();
    }
}