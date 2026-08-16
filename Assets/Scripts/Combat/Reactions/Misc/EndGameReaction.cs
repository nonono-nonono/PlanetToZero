using UnityEngine;

// Ends the game
public class EndGameReaction : ReactionBase
{
    public override void Execute(EventContext ctx)
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        GameManager.Instance.EndGame();
    }
}
