using System.Collections.Generic;
using UnityEngine;

public class CubeLookTrigger : MonoBehaviour, ILookTrigger
{
    [Tooltip("Scene objects to deactivate when looked at")]
    [SerializeField] private List<GameObject> objectsToDeactivate = new List<GameObject>();

    [Tooltip("Scene objects to activate when looked at")]
    [SerializeField] private List<GameObject> objectsToActivate = new List<GameObject>();

    [Tooltip("If true, only trigger once")]
    [SerializeField] private bool oneShot = true;

    private bool _hasTriggered;

    public void OnLookEnter(GameObject instigator)
    {
        if (oneShot && _hasTriggered) return;

        foreach (var go in objectsToDeactivate)
        {
            if (go != null && go.activeSelf)
                go.SetActive(false);
        }

        foreach (var go in objectsToActivate)
        {
            if (go != null && !go.activeSelf)
                go.SetActive(true);
        }

        _hasTriggered = true;
    }

    public void OnLookExit(GameObject instigator)
    {
        // optional: reset for repeated triggers
        // if (!oneShot) _hasTriggered = false;
    }

    // optional API to reset the one-shot from other code / editor
    public void ResetTrigger() => _hasTriggered = false;
}