using System;
using System.Collections.Generic;
using UnityEngine;
using Events;

// TODO: Maybe instead of different events for each state, have a single event that passes the "script state" to be executed?
public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Intro,
        Level1,
        Level2,
        End
    }

    private List<GameState> gameStates;
    [SerializeField] private int currentStateIndex = 0;

    private void Awake()
    {
        // Automatically populate the list with all enum values
        gameStates = new List<GameState>((GameState[])Enum.GetValues(typeof(GameState)));
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
        // Initialize the game in the first state
        TransitionToState(gameStates[currentStateIndex]);
    }

    private void HandleStartNextGameState()
    {
        if (currentStateIndex < gameStates.Count - 1)
        {
            currentStateIndex++;
            TransitionToState(gameStates[currentStateIndex]);
        }
        else
        {
            Debug.Log("No more game states to transition to.");
        }
    }

    private void TransitionToState(GameState state)
    {
        Debug.Log($"Transitioning to state: {state}");

        // Broadcast the corresponding event for the current state
        switch (state)
        {
            case GameState.Intro:
                GameEvents.BeginIntro();
                break;

            case GameState.Level1:
                GameEvents.BeginLevel1();
                break;

            case GameState.Level2:
                GameEvents.BeginLevel2();
                break;

            case GameState.End:
                GameEvents.BeginEnding();
                break;

            default:
                Debug.LogError($"Unhandled game state: {state}");
                break;
        }
    }
}