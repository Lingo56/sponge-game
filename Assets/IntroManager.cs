using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public GameObject introTextParent;
    public Image introPanel;

    public float enterStartFade;
    public float enterEndFade;
    public float startFadeLength;
    public float exitStartFade;
    public float exitEndFade;
    public float exitFadeLength;

    private TMP_Text[] allTMPs;

    private InputAction startAction;

    void Start()
    {
        StartCoroutine(OnLoadEvent());
    }

    void Awake()
    {
        allTMPs = introTextParent.GetComponentsInChildren<TMP_Text>(true);

        startAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/e");
        startAction.Enable();
    }

    void Update()
    {
        if (startAction.WasPressedThisFrame())
        {
            StartCoroutine(PlayerStartEvent());
        }
    }

    IEnumerator OnLoadEvent()
    {
        StartCoroutine(FadeUI(enterEndFade, enterEndFade, startFadeLength, introPanel.material));

        foreach (var tmpObject in allTMPs)
        {
            tmpObject.fontMaterial = new Material(tmpObject.fontSharedMaterial);
            yield return StartCoroutine(FadeUI(enterStartFade, enterEndFade, startFadeLength, tmpObject));
        }
    }

    IEnumerator PlayerStartEvent()
    {
        StartCoroutine(FadeUI(exitStartFade, exitEndFade, exitFadeLength, introPanel.material));

        foreach (var tmpObject in allTMPs)
        {
            StartCoroutine(FadeUI(exitStartFade, exitEndFade, exitFadeLength, tmpObject));
        }
        yield break;
    }

    IEnumerator FadeUI(float startValue, float endValue, float duration, TMP_Text tmp_asset)
    {
        float elapsed = 0f;
        var mat = tmp_asset.fontMaterial;
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
    
    IEnumerator FadeUI(float startValue, float endValue, float duration, Material mat)
    {
        float elapsed = 0f;
        if (!mat.HasProperty("_Dissolve"))
            yield break;
        while (elapsed < duration)
        {
            float value = Mathf.Lerp(startValue, endValue, elapsed / duration);
            mat.SetFloat("_Dissolve", value);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mat.SetFloat("_Dissolve", endValue);
    }
}
