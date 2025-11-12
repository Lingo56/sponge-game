using System;
using Events;
using Objects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 0.5f;
        private float _xRotation;
        private bool lookEnabled;
        [SerializeField] private Camera playerCamera;
        
        private IInteractable currentHovered;
        
        private void Start()
        {
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            GameEvents.OnEnableMouseLook += EnableMouseLook;
            GameEvents.OnDisableMouseLook += DisableMouseLook;
        }

        private void OnDisable()
        {
            GameEvents.OnEnableMouseLook -= EnableMouseLook;
            GameEvents.OnDisableMouseLook -= DisableMouseLook;
        }

        private void EnableMouseLook()
        {
            lookEnabled = true;
        }

        private void DisableMouseLook()
        {
            lookEnabled = false;
        }

        void Update()
        {
            if (!lookEnabled) return;

            Vector2 mouseDelta = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
            _xRotation -= mouseDelta.y * sensitivity;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.parent.Rotate(Vector3.up * (mouseDelta.x * sensitivity));
            
            IInteractable hitInteractable = null;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out var hit, 3f))
            {
                if (hit.collider != null && hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    hitInteractable = interactable;
                }
            }

            if (hitInteractable != currentHovered)
            {
                if (currentHovered != null)
                {
                    GameEvents.InteractableHoverExit(currentHovered);
                }

                if (hitInteractable != null)
                {
                    GameEvents.InteractableHoverEnter(hitInteractable);
                }

                currentHovered = hitInteractable;
            }
        }
    }
}