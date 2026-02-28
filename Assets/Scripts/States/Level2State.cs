using Events;
using UnityEngine;

public class Level2State : MonoBehaviour
{
    private bool advanceInteracted = false;
    [SerializeField] private GameObject spawnPoint;
    
    // Toggleable level objects
    [SerializeField] private GameObject cube1FrontDoor;
    [SerializeField] private GameObject cube1NoDoor;
    [SerializeField] private GameObject cube1BackDoor;
    
    [SerializeField] private GameObject cube2FrontDoor;
    [SerializeField] private GameObject cube2Window;
    [SerializeField] private GameObject cube2BackDoor;
    
    [SerializeField] private GameObject cube3FrontDoor;

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
        cube1FrontDoor.SetActive(true);
    }
    

    public string LevelAdvanceString { get; }
}

