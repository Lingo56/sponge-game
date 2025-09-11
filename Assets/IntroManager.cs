using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class IntroManager : MonoBehaviour
{
    public TMP_Text introText;

    public float startFade;
    public float endFade;
    public float fadeLength;

    private InputAction startAction;

    void Awake()
    {
        startAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/e");
        startAction.Enable();
    }

    void Update()
    {
        if (startAction.WasPressedThisFrame())
        {
            StartCoroutine(FadeText(startFade, endFade, fadeLength));
        }
    }
    
    IEnumerator FadeText(float startValue, float endValue, float duration)
    {
        float elapsed = 0f;
        var mat = introText.fontSharedMaterial;
        if (!mat.HasProperty("_Dissolve"))
        {
            Debug.LogError($"Material does not have _Dissolve property! Shader in use: {mat.shader.name}");
            yield break;
        }
        while (elapsed < duration)
        {
            float value = Mathf.Lerp(startValue, endValue, elapsed / duration);
            Debug.Log($"Setting _Dissolve to {value}");
            mat.SetFloat("_Dissolve", value);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Ensure final value is set
        mat.SetFloat("_Dissolve", endValue);
    }
}
