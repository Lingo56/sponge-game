using System.Collections;
using System.Collections.Generic;
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
        
        // Cache of materials so we can clean them up properly
        private readonly List<TMP_Text> ownedTexts = new List<TMP_Text>();
        private readonly List<Material> ownedMaterials = new List<Material>();
        private Material ownedPanelMaterial;

        private void OnEnable()
        {
            // Use event subscriptions as before
            GameEvents.OnBeginIntro += StartIntroState;
            InputHandler.OnInteract += HandleIntroInteraction;

            // Subscribe to generic state-changed event so we can abort/clear intro whenever the game state changes
            GameEvents.OnGameStateChanged += OnAnyGameStateChanged;
        }

        private void OnDisable()
        {
            // Unsubscribe first to avoid receiving events while cleaning up
            GameEvents.OnBeginIntro -= StartIntroState;
            InputHandler.OnInteract -= HandleIntroInteraction;

            // Unsubscribe generic state change event
            GameEvents.OnGameStateChanged -= OnAnyGameStateChanged;

            // Safely attempt cleanup of any materials we created/own. Wrap in try/catch to be robust during domain reload/shutdown.
            CleanupOwnedMaterials();
        }

        private void OnDestroy()
        {
            // Ensure we clean up if the object is destroyed directly
            CleanupOwnedMaterials();
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

            // Create and assign owned material instances for panel and text children
            InitializeIntroTextMaterials();

            if (ownedPanelMaterial != null)
            {
                StartCoroutine(FadeUI(enterEndFade, enterEndFade, startFadeLength, ownedPanelMaterial));
            }

            // Fade each owned text (we created their fontMaterial in InitializeIntroTextMaterials)
            foreach (var tmpObject in ownedTexts)
            {
                if (tmpObject == null) continue;
                yield return StartCoroutine(FadeUI(enterStartFade, enterEndFade, startFadeLength, tmpObject));
            }

            canStartGameInteract = true;
        }

        private IEnumerator HandleGameStartFadeOut()
        {
            canStartGameInteract = false;

            if (ownedPanelMaterial != null)
            {
                StartCoroutine(FadeUI(exitStartFade, exitEndFade, exitFadeLength, ownedPanelMaterial));
            }

            foreach (var tmpObject in ownedTexts)
            {
                if (tmpObject == null) continue;
                StartCoroutine(FadeUI(exitStartFade, exitEndFade, exitFadeLength, tmpObject));
            }

            yield return new WaitForSeconds(exitFadeLength);
            GameEvents.StartNextGameState(); // Trigger the game start event
        }

        private IEnumerator FadeUI(float startValue, float endValue, float duration, TMP_Text tmpAsset)
        {
            if (tmpAsset == null)
                yield break;

            float elapsed = 0f;
            var mat = tmpAsset.fontMaterial;
            if (mat == null || !mat.HasProperty("_Dissolve"))
            {
                // If material doesn't support dissolve, just ensure it's at endValue if possible
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
            if (mat == null || !mat.HasProperty("_Dissolve"))
                yield break;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                float value = Mathf.Lerp(startValue, endValue, elapsed / duration);
                mat.SetFloat("_Dissolve", value);
                elapsed += Time.deltaTime;
                yield return null;
            }
            mat.SetFloat("_Dissolve", endValue);
        }

        // Clones the shared materials onto the UI elements so that we can modify them without changes sticking outside play mode.
        private void InitializeIntroTextMaterials()
        {
            // Clean previous owned caches if any (defensive)
            CleanupOwnedMaterials();

            // Panel: create an owned material instance so we can safely modify it
            if (introPanel != null && introPanel.material != null)
            {
                try
                {
                    ownedPanelMaterial = new Material(introPanel.material);
                    introPanel.material = ownedPanelMaterial;
                    ownedMaterials.Add(ownedPanelMaterial);
                    // start invisible
                    if (ownedPanelMaterial.HasProperty("_Dissolve"))
                        ownedPanelMaterial.SetFloat("_Dissolve", 1);
                }
                catch
                {
                    // If anything goes wrong (shutdown/reload), bail gracefully
                    ownedPanelMaterial = null;
                }
            }

            // Create owned materials for each TMP child and cache them
            if (introTextParent == null) return;

            TMP_Text[] tmpChildren;
            try
            {
                tmpChildren = introTextParent.GetComponentsInChildren<TMP_Text>(true);
            }
            catch
            {
                // If the parent/gameobjects are in the process of being destroyed, just return
                return;
            }

            foreach (var tmpObject in tmpChildren)
            {
                if (tmpObject == null || tmpObject.fontSharedMaterial == null) continue;
                try
                {
                    var matInstance = new Material(tmpObject.fontSharedMaterial);
                    tmpObject.fontMaterial = matInstance;
                    ownedTexts.Add(tmpObject);
                    ownedMaterials.Add(matInstance);

                    // Ensure starting state is invisible if property exists
                    if (matInstance.HasProperty("_Dissolve"))
                        matInstance.SetFloat("_Dissolve", 1);
                }
                catch
                {
                    // ignore problems when creating materials during shutdown
                }
            }
        }
        
        // Sets every "intro owned" material invisible and destroys them.
        private void CleanupOwnedMaterials()
        {
            // Set all owned materials to invisible and destroy them. Use try/catch to avoid errors during shutdown.
            try
            {
                // Panel
                if (ownedPanelMaterial != null)
                {
                    if (ownedPanelMaterial.HasProperty("_Dissolve"))
                        ownedPanelMaterial.SetFloat("_Dissolve", 1);
                }

                // Texts
                for (int i = 0; i < ownedTexts.Count; i++)
                {
                    var tmp = ownedTexts[i];
                    if (tmp == null) continue;
                    var mat = tmp.fontMaterial;
                    if (mat != null && mat.HasProperty("_Dissolve"))
                        mat.SetFloat("_Dissolve", 1);
                }

                // Destroy materials we created
                for (int i = 0; i < ownedMaterials.Count; i++)
                {
                    var mat = ownedMaterials[i];
                    if (mat == null) continue;
#if UNITY_EDITOR
                    // Use DestroyImmediate in editor to avoid leaking during domain reload/stop
                    UnityEngine.Object.DestroyImmediate(mat);
#else
                    UnityEngine.Object.Destroy(mat);
#endif
                }
            }
            catch (System.Exception)
            {
                // Swallow exceptions during application shutdown or domain reload — nothing we can safely do
            }
            finally
            {
                ownedMaterials.Clear();
                ownedTexts.Clear();
                ownedPanelMaterial = null;
            }
        }

        // Clear all intro assets if state changes
        private void OnAnyGameStateChanged(GameManager.GameState newState)
        {
            // Only clear if we're transitioning away from Intro
            if (newState == GameManager.GameState.Intro) return;

            try { StopAllCoroutines(); } catch { }
            canStartGameInteract = false;
            CleanupOwnedMaterials();
        }
    }
}
