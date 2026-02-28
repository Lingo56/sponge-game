using Events;
using UnityEngine;

namespace Player
{
    [DisallowMultipleComponent]
    public class LookRaycaster : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private float maxDistance = 3f;
        [SerializeField] private LayerMask layerMask = ~0;

        private ILookTrigger _current;
        private InteractableComponent _currentInteractable;

        private void Awake()
        {
            if (targetCamera == null) targetCamera = Camera.main;
        }

        private void Update()
        {
            if (targetCamera == null) return;

            if (Physics.Raycast(targetCamera.transform.position, targetCamera.transform.forward, out var hit, maxDistance, layerMask))
            {
                // find an ILookTrigger on the hit or its parents
                ILookTrigger found = null;
                var comps = hit.collider.GetComponentsInParent<MonoBehaviour>(true);
                foreach (var c in comps)
                {
                    if (c is ILookTrigger lt) { found = lt; break; }
                }

                // forward existing Interactable hover events for compatibility
                InteractableComponent foundInteractable = null;
                if (hit.collider && hit.collider.TryGetComponent<InteractableComponent>(out var interactable))
                    foundInteractable = interactable;

                if (found != _current)
                {
                    _current?.OnLookExit(gameObject);
                    _current = found;
                    _current?.OnLookEnter(gameObject);
                }

                if (foundInteractable != _currentInteractable)
                {
                    if (_currentInteractable != null) GameEvents.InteractableHoverExit(_currentInteractable);
                    if (foundInteractable != null) GameEvents.InteractableHoverEnter(foundInteractable);
                    _currentInteractable = foundInteractable;
                }
            }
            else
            {
                if (_current != null)
                {
                    _current.OnLookExit(gameObject);
                    _current = null;
                }

                if (_currentInteractable != null)
                {
                    GameEvents.InteractableHoverExit(_currentInteractable);
                    _currentInteractable = null;
                }
            }
        }
    }
}
