using UnityEngine;

public class EndGameReaction : ReactionBase
{
    public override void Execute(EventContext ctx)
    {
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        GameManager.Instance.EndGame();
    }
}
