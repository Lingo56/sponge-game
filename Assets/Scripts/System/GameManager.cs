using System.Collections;
using UnityEngine;
using Events;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Intro,
        Level1,
        Level2,
        End
    }

    [SerializeField] private GameState currentState;

    // Tracks whether the GameManager has applied the inspector state and is ready for OnValidate-driven transitions
    private bool isInitialized;

    // Track the last transition that actually executed so we can skip redundant transitions
    private GameState lastTransitionedState;

    // Pending transition state
    private GameState pendingRequestedState;
    private bool pendingForce;
    private Coroutine pendingTransitionCoroutine;

    public void SetState(GameState newState, bool forceTransition = false)
    {
        // If nothing changed and we're not forcing a transition, do nothing
        if (newState == currentState && !forceTransition) return;

        // Update the authoritative state and notify listeners. This is the single source of truth they can query.
        currentState = newState;
        GameEvents.GameStateChanged(currentState);

        // Setup pending variables to resolve race condition
        pendingRequestedState = newState;
        pendingForce = forceTransition || pendingForce;

        if (pendingTransitionCoroutine == null)
            pendingTransitionCoroutine = StartCoroutine(PerformPendingTransition());
    }

    // Solves state change race condition. Prioritizes forced states, then latest state
    private IEnumerator PerformPendingTransition()
    {
        // Wait one frame so every SetState call on the prev frame runs
        yield return null;

        // Capture and clear pending info
        var target = pendingRequestedState;
        var force = pendingForce;
        pendingRequestedState = default;
        pendingForce = false;
        pendingTransitionCoroutine = null;

        // If this is a redundant transition and not forced, skip it
        if (target == lastTransitionedState && !force)
            yield break;

        // Otherwise run the transition
        TransitionToState(target);
        lastTransitionedState = target;
    }

    private void OnEnable()
    {
        GameEvents.OnStartNextGameState += HandleStartNextGameState;
    }

    private void OnDisable()
    {
        GameEvents.OnStartNextGameState -= HandleStartNextGameState;
    }

    private void Start()
    {
        // Wait one frame for this to run so that any listeners have a chance to subscribe first
        StartCoroutine(ApplyInspectorStateNextFrame());
    }

    private IEnumerator ApplyInspectorStateNextFrame()
    {
        yield return null; // wait one frame
        SetState(currentState, true);
        isInitialized = true;
    }

    private void HandleStartNextGameState()
    {
        int next = ((int)currentState + 1) % System.Enum.GetValues(typeof(GameState)).Length;
        SetState((GameState)next);
    }
    
    // Is called by Unity when editor changes are made
    private void OnValidate()
    {
// Skips this code if it's compiled to run outside of Unity Editor
#if UNITY_EDITOR 
        // Force state change from editor in play mode
        if (Application.isPlaying && isInitialized)
            SetState(currentState, true);
#endif
    }

    private void TransitionToState(GameState state)
    {
        Debug.Log($"Transitioning to state: {state}");
        switch (state)
        {
            case GameState.Intro:   GameEvents.BeginIntro(); break;
            case GameState.Level1:  GameEvents.BeginLevel1(); break;
            case GameState.Level2:  GameEvents.BeginLevel2(); break;
            case GameState.End:     GameEvents.BeginEnding(); break;
            default: Debug.LogError($"Unhandled game state: {state}"); break;
        }
    }
}