using System;
using Objects;
using UnityEngine;

namespace Events
{
    public static class GameEvents
    {
        // State Transition Events
        public static event Action OnStartNextGameState; 
        public static event Action OnBeginIntro; 
        public static event Action OnBeginLevel1; 
        public static event Action OnBeginLevel2; 
        public static event Action OnBeginLevelEnding; 
        
        public static void StartNextGameState() => OnStartNextGameState?.Invoke();
        public static void BeginIntro() => OnBeginIntro?.Invoke();
        public static void BeginLevel1() => OnBeginLevel1?.Invoke();
        public static void BeginLevel2() => OnBeginLevel2?.Invoke();
        public static void BeginEnding() => OnBeginLevelEnding?.Invoke();
        
        // Input Events
        public static event Action OnEnableMouseLook;
        public static event Action OnEnablePlayerMovement;
        public static event Action OnDisableMouseLook;
        public static event Action OnDisablePlayerMovement;
        public static event Action<GameObject> OnSetPlayerSpawnPoint;
        // Added position-based spawn event to avoid referencing prefab assets in inspector
        public static event Action<Vector3> OnSetPlayerSpawnPosition;
        public static event Action<InteractableComponent> OnInteractableHoverEnter;
        public static event Action<InteractableComponent> OnInteractableHoverExit;
        
        public static void EnableMouseLook() => OnEnableMouseLook?.Invoke();
        public static void EnablePlayerMovement() => OnEnablePlayerMovement?.Invoke();
        public static void DisableMouseLook() => OnDisableMouseLook?.Invoke();
        public static void DisablePlayerMovement() => OnDisablePlayerMovement?.Invoke();
        public static void SetPlayerSpawnPoint(GameObject spawnPoint) => OnSetPlayerSpawnPoint?.Invoke(spawnPoint);
        public static void SetPlayerSpawnPointPosition(Vector3 spawnPosition) => OnSetPlayerSpawnPosition?.Invoke(spawnPosition);
        public static void InteractableHoverEnter(InteractableComponent interactable) => OnInteractableHoverEnter?.Invoke(interactable);
        public static void InteractableHoverExit(InteractableComponent interactable) => OnInteractableHoverExit?.Invoke(interactable);
    }
}