using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class Level1State : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.OnBeginLevel1 += StartLevelState;
        InputHandler.OnInteract += HandleLevelInteraction;
    }

    private void OnDisable()
    {
        GameEvents.OnBeginLevel1 -= StartLevelState;
        InputHandler.OnInteract -= HandleLevelInteraction;
    }
    
    private void StartLevelState()
    {
        Debug.Log("Entering Level1...");
        GameEvents.EnablePlayerMovement();
        GameEvents.EnableMouseLook();
    }

    private void HandleLevelInteraction()
    {
        throw new System.NotImplementedException();
    }
}
