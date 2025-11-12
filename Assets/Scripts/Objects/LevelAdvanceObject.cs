using Events;
using TMPro;
using UnityEngine;

namespace Objects
{
    // TODO: Need to add safety here so that player can't spam and advance many levels
    public class LevelAdvanceObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private TextMeshProUGUI debugText;

        private void OnEnable()
        {
            GameEvents.OnInteractableHoverEnter += OnHoverEnter;
            GameEvents.OnInteractableHoverExit += OnHoverExit;
        }

        private void OnDisable()
        {
            GameEvents.OnInteractableHoverEnter -= OnHoverEnter;
            GameEvents.OnInteractableHoverExit -= OnHoverExit;
        }

        // Parameterized handlers (match events that pass IInteractable)
        private void OnHoverEnter(IInteractable interactable)
        {
            if (interactable == this) SetDebugVisible(true);
        }

        private void OnHoverExit(IInteractable interactable)
        {
            if (interactable == this) SetDebugVisible(false);
        }

        private void SetDebugVisible(bool visible)
        {
            if (debugText) debugText.enabled = visible;
        }

        // Keep IInteractable API compatibility
        public void InteractEnter()
        {
            SetDebugVisible(true);
        }

        public void InteractExit()
        {
            SetDebugVisible(false);
        }
    }
}