using UnityEngine;
using UnityEngine.InputSystem;
    
public class MouseLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.5f;
    private float _xRotation;
    private bool lookEnabled = false;

    private void OnEnable()
    {
        InputHandler.OnToggleMouseLook += ToggleMouseLook;
    }

    private void OnDisable()
    {
        InputHandler.OnToggleMouseLook -= ToggleMouseLook;
    }

    public void ToggleMouseLook()
    {
        lookEnabled = !lookEnabled; // Toggle the state
    }

    void Update()
    {
        if (!lookEnabled) return;

        Vector2 mouseDelta = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
        _xRotation -= mouseDelta.y * sensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.parent.Rotate(Vector3.up * (mouseDelta.x * sensitivity));
    }
}