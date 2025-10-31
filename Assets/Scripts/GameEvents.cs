using System;

public static class GameEvents
{
    public static event Action OnPlayerStartGame; // Player presses E to start the game
    public static event Action OnSecondLevelStart; // Player interacts and beginns second level
    public static event Action OnGameEnd; // Start game end sequence (credits etc..)
    
    public static void PlayerStartGame() => OnPlayerStartGame?.Invoke();
    public static void SecondLevelStart() => OnSecondLevelStart?.Invoke();
    public static void GameEndStart() => OnGameEnd?.Invoke();
}