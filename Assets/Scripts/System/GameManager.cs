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

    public GameState CurrentState
    {
        get => currentState;
        set => SetState(value);
    }

    // Numeric compatibility for other systems
    public int CurrentStateIndex
    {
        get => (int)currentState;
        set => SetState((GameState)value);
    }

    public void SetState(GameState newState, bool forceTransition = false)
    {
        if (newState == currentState && !forceTransition) return;
        currentState = newState;
        TransitionToState(currentState);
    }

    private void OnEnable()
    {
        GameEvents.OnStartNextGameState += HandleStartNextGameState;
    }

    private void OnDisable()
    {
        GameEvents.OnStartNextGameState -= HandleStartNextGameState;
    }

    private void HandleStartNextGameState()
    {
        int next = ((int)currentState + 1) % System.Enum.GetValues(typeof(GameState)).Length;
        SetState((GameState)next);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        // When editing the dropdown during Play, force the transition
        if (Application.isPlaying)
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