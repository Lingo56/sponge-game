// File: `Assets/Scripts/Player/MouseLook.cs`
using Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 0.5f;
        private float _xRotation;
        [SerializeField] private bool lookEnabled;
        [SerializeField] private Camera playerCamera;

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
            if (transform.parent != null)
                transform.parent.Rotate(Vector3.up * (mouseDelta.x * sensitivity));
        }
    }
}