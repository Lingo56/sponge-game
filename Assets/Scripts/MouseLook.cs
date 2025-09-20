using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public bool lookEnabled = false;

    [SerializeField] private float sensitivity = 0.5f;
    [SerializeField] private float safariSmoothingFactor = 0.2f; // Lower is smoother (0.01-0.25 is a good range)
    [SerializeField] private bool applySmoothingToSafariOnly = true;
    
    private float _xRotation = 0f;
    private Keyboard keyboard => Keyboard.current;
    private Mouse mouse => Mouse.current;
    private bool _isSafari = false;
    
    // Smoothing variables
    private Vector2 _currentMouseDelta = Vector2.zero;
    private Vector2 _targetMouseDelta = Vector2.zero;

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void InitializePointerLock();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern float GetMouseDeltaX();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern float GetMouseDeltaY();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool IsSafari();

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        #if !UNITY_EDITOR && UNITY_WEBGL
            InitializePointerLock();
            _isSafari = IsSafari();
        #endif
    }

    void Update()
    {
        if (!lookEnabled) return;

        Vector2 rawMouseDelta;
        #if !UNITY_EDITOR && UNITY_WEBGL
                    rawMouseDelta = new Vector2(GetMouseDeltaX(), GetMouseDeltaY());
        #else
                    rawMouseDelta = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
        #endif

        // Apply browser-specific sensitivity
        float effectiveSensitivity = sensitivity;
        #if !UNITY_EDITOR && UNITY_WEBGL
            if (_isSafari)
            {
                effectiveSensitivity *= safariSensitivityMultiplier;
            }
        #endif

        rawMouseDelta *= effectiveSensitivity;

        // Determine if we should apply smoothing
        bool applySmoothing = (_isSafari && applySmoothingToSafariOnly) || !applySmoothingToSafariOnly;

        Vector2 mouseDelta;
        if (applySmoothing)
        {
            // Set the target to the raw input
            _targetMouseDelta = rawMouseDelta;

            // Smoothly interpolate current value toward target
            _currentMouseDelta = Vector2.Lerp(_currentMouseDelta, _targetMouseDelta, safariSmoothingFactor);

            // Use smoothed value
            mouseDelta = _currentMouseDelta;
        }
        else
        {
            // Use raw input directly
            mouseDelta = rawMouseDelta;
        }

        if (mouseDelta.magnitude > 0)
        {
            _xRotation -= mouseDelta.y;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.parent.Rotate(Vector3.up * mouseDelta.x);
        }
    }
    
}