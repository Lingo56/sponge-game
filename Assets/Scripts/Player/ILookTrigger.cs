using UnityEngine;

public interface ILookTrigger
{
    void OnLookEnter(GameObject instigator);
    void OnLookExit(GameObject instigator);
}