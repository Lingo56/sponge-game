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
        GameEvents.EnablePlayerMovement();
        GameEvents.EnableMouseLook();
    }

    // TODO: Make it so that this starts the next state when interacting with the right spot
    // Start with it being in the right spot then maybe implement a hover system 
    private void HandleLevelInteraction()
    {
        if (true)
        {
            GameEvents.StartNextGameState(); // Trigger the game start event
        }
    }
}
