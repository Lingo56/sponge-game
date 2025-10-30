using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private GameObject introTextParent;
    [SerializeField] private Image introPanel;
    [SerializeField] private MouseLook mouseControl;
    [SerializeField] private float enterStartFade;
    [SerializeField] private float enterEndFade;
    [SerializeField] private float startFadeLength;
    [SerializeField] private float exitStartFade;
    [SerializeField] private float exitEndFade;
    [SerializeField] private float exitFadeLength;

    private TMP_Text[] allTMPs;
    private bool canStartGameInteract;

    private void Start()
    {
        StartCoroutine(HandleIntroFadeIn());
    }

    private void OnEnable()
    {
        InputHandler.OnInteract += HandleIntroInteraction;
    }

    private void OnDisable()
    {
        InputHandler.OnInteract -= HandleIntroInteraction;
    }

    private void HandleIntroInteraction()
    {
        if (canStartGameInteract)
        {
            StartCoroutine(HandleGameStartFadeOut());
        }
    }

    private IEnumerator HandleIntroFadeIn()
    {
        StartCoroutine(FadeUI(enterEndFade, enterEndFade, startFadeLength, introPanel.material));

        foreach (var tmpObject in introTextParent.GetComponentsInChildren<TMP_Text>(true))
        {
            tmpObject.fontMaterial = new Material(tmpObject.fontSharedMaterial);
            yield return StartCoroutine(FadeUI(enterStartFade, enterEndFade, startFadeLength, tmpObject));
        }

        canStartGameInteract = true;
    }

    private IEnumerator HandleGameStartFadeOut()
    {
        canStartGameInteract = false;

        StartCoroutine(FadeUI(exitStartFade, exitEndFade, exitFadeLength, introPanel.material));

        foreach (var tmpObject in introTextParent.GetComponentsInChildren<TMP_Text>(true))
        {
            StartCoroutine(FadeUI(exitStartFade, exitEndFade, exitFadeLength, tmpObject));
        }

        yield return new WaitForSeconds(exitFadeLength);
        GameEvents.PlayerStartGame(); // Trigger the game start event
        mouseControl.ToggleMouseLook(); // TODO: Set this up to be less jank -.-
    }

    private IEnumerator FadeUI(float startValue, float endValue, float duration, TMP_Text tmpAsset)
    {
        float elapsed = 0f;
        var mat = tmpAsset.fontMaterial;
        if (!mat.HasProperty("_Dissolve"))
        {
            Debug.LogError($"Material does not have _Dissolve property! Shader in use: {mat.shader.name}");
            yield break;
        }
        while (elapsed < duration)
        {
            float value = Mathf.Lerp(startValue, endValue, elapsed / duration);
            mat.SetFloat("_Dissolve", value);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mat.SetFloat("_Dissolve", endValue);
    }

    private IEnumerator FadeUI(float startValue, float endValue, float duration, Material mat)
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