using Events;
using UnityEngine;

public class Level2State : MonoBehaviour
{
    private bool advanceInteracted = false;
    [SerializeField] private GameObject spawnPoint;
    
    private void OnEnable()
    {
        GameEvents.OnBeginLevel2 += StartLevelState;
    }

    private void OnDisable()
    {
        GameEvents.OnBeginLevel2 -= StartLevelState;
    }
    
    private void StartLevelState()
    {
        GameEvents.EnablePlayerMovement();
        GameEvents.EnableMouseLook();
        GameEvents.SetPlayerSpawnPoint(spawnPoint);
    }

    public string LevelAdvanceString { get; }
}
