using UnityEngine;
using Events;

[CreateAssetMenu(menuName = "Interactions/LevelAdvanceAction")]
public class LevelAdvanceAction : InteractionAction
{
    [SerializeField] private string hoverText = "Advance";

    public override bool Execute(GameObject instigator)
    {
        // Raise the game event to advance state; listeners (like Level1State) will handle specifics.
        GameEvents.StartNextGameState();
        return true;
    }

    public override string GetHoverText(GameObject instigator) => hoverText;
}