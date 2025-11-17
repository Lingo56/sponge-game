using UnityEngine;
using TMPro;
using Events;

public class InteractableComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hoverUIText;
    [SerializeField] private string hoverText = "Interact";
    [SerializeField] private InteractionAction action;
    [SerializeField] private bool singleUse = false;
    [SerializeField] private float cooldownSeconds = 0f;

    private bool used;
    private float lastUsedTime = -Mathf.Infinity;

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

    private void OnHoverEnter(InteractableComponent interactable)
    {
        if (interactable != this) return;
        string textFromAction = action?.GetHoverText(gameObject);
        string textToShow = string.IsNullOrEmpty(textFromAction) ? hoverText : textFromAction;
        SetHoverTextVisible(true, textToShow);
    }

    private void OnHoverExit(InteractableComponent interactable)
    {
        if (interactable == this) SetHoverTextVisible(false);
    }

    private void SetHoverTextVisible(bool visible, string text = null)
    {
        if (!hoverUIText) return;
        hoverUIText.enabled = visible;
        hoverUIText.text = visible ? (text ?? hoverText) : "";
    }

    public void InteractEnter()
    {
        if (singleUse && used) return;
        if (Time.time - lastUsedTime < cooldownSeconds) return;
        if (action == null) return;
        if (!action.CanExecute(gameObject)) return;

        bool executed = action.Execute(gameObject);
        lastUsedTime = Time.time;
        if (executed && singleUse) used = true;
    }

    public void InteractExit() { /* optional: cancel hold interactions */ }
}