using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Interactions/UnityEventAction")]
public class UnityEventAction : InteractionAction
{
    [SerializeField] private UnityEvent<GameObject> onExecute;
    [SerializeField] private string hoverText;

    public override bool Execute(GameObject instigator)
    {
        if (onExecute == null) return false;
        onExecute.Invoke(instigator);
        return true;
    }

    public override bool CanExecute(GameObject instigator)
    {
        return onExecute != null && onExecute.GetPersistentEventCount() > 0;
    }

    public override string GetHoverText(GameObject instigator)
    {
        return string.IsNullOrEmpty(hoverText) ? null : hoverText;
    }
}