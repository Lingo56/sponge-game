using System.Collections;
using Events;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace States
{
    public class IntroState : MonoBehaviour
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

        private void OnEnable()
        {
            DisableIntroTextMaterials();
            GameEvents.OnBeginIntro += StartIntroState;
            InputHandler.OnInteract += HandleIntroInteraction;
        }

        private void OnDisable()
        {
            GameEvents.OnBeginIntro -= StartIntroState;
            InputHandler.OnInteract -= HandleIntroInteraction;
            
            // Only attempt cleanup if objects still exist (not destroyed during shutdown)
            if (introPanel != null || introTextParent != null)
            {
                DisableIntroTextMaterials();
            }
        }
        
        private void StartIntroState()
        {
            StartCoroutine(HandleIntroFadeIn());
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
            Debug.Log("Entering Intro state...");
            GameEvents.DisableMouseLook();
            GameEvents.DisablePlayerMovement();
            InitializeIntroTextMaterials();
            
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
            GameEvents.StartNextGameState(); // Trigger the game start event
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
        
        private void InitializeIntroTextMaterials()
        {
            // Set panel to fully visible immediately
            if (introPanel && introPanel.material)
            {
                introPanel.material.SetFloat("_Dissolve", 0);
            }
        }
        
        private void DisableIntroTextMaterials()
        {
            // Set panel to fully invisible (check for null in case it's destroyed)
            if (introPanel != null && introPanel.material != null)
            {
                introPanel.material.SetFloat("_Dissolve", 1);
            }
    
            // Set all text to invisible (check for null in case it's destroyed)
            if (introTextParent == null) return;
            
            var tmpChildren = introTextParent.GetComponentsInChildren<TMP_Text>(true);
            if (tmpChildren == null) return;
            
            foreach (var tmpObject in tmpChildren)
            {
                if (tmpObject == null || tmpObject.fontSharedMaterial == null) continue;
                
                tmpObject.fontMaterial = new Material(tmpObject.fontSharedMaterial);
                if (tmpObject.fontMaterial != null && tmpObject.fontMaterial.HasProperty("_Dissolve"))
                {
                    tmpObject.fontMaterial.SetFloat("_Dissolve", 1);
                }
            }
        }
    }
}