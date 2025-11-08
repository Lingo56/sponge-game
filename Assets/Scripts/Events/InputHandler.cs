using UnityEngine;
using UnityEngine.InputSystem;

namespace Events
{
    public class InputHandler : MonoBehaviour
    {
        private InputAction interactAction;

        public static event System.Action OnInteract; // Generic interact event

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
}