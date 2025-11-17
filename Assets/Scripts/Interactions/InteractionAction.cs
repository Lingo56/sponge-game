using UnityEngine;

public abstract class InteractionAction : ScriptableObject
{
    // Execute the action; return true if it succeeded.
    public abstract bool Execute(GameObject instigator);

    // Optional pre-check before execute.
    public virtual bool CanExecute(GameObject instigator) => true;

    // Optional hover text override provided by the action.
    // Return null or empty to indicate no override.
    public virtual string GetHoverText(GameObject instigator) => null;
}