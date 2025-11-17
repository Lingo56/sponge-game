using Events;
using UnityEngine;

public class Level1State : MonoBehaviour
{
    private bool advanceInteracted = false;
    
    private void OnEnable()
    {
        GameEvents.OnBeginLevel1 += StartLevelState;
    }

    private void OnDisable()
    {
        GameEvents.OnBeginLevel1 -= StartLevelState;
    }
    
    private void StartLevelState()
    {
        GameEvents.EnablePlayerMovement();
        GameEvents.EnableMouseLook();
    }

    public string LevelAdvanceString { get; }
}
