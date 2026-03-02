using Events;
using UnityEngine;

public class Level1State : MonoBehaviour
{
    private bool advanceInteracted = false;
    [SerializeField] private GameObject spawnPoint;
    
    private void OnEnable()
    {
        GameEvents.OnBeginLevel1 += StartLevelState;
        InputHandler.OnInteract += HandleState1Interaction;
    }

    private void OnDisable()
    {
        GameEvents.OnBeginLevel1 -= StartLevelState;
        InputHandler.OnInteract += HandleState1Interaction;
    }
    
    private void StartLevelState()
    {
        GameEvents.EnablePlayerMovement();
        GameEvents.EnableMouseLook();
        GameEvents.SetPlayerSpawnPoint(spawnPoint);
    }

    private void HandleState1Interaction()
    {
        
    }

    public string LevelAdvanceString { get; }
}
