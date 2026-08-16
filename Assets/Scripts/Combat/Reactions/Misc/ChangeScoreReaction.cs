using UnityEngine;

// Adds score to the score manager.
public class ChangeScoreReaction : ReactionBase
{
    public float ScoreChange;
    public override void Execute(EventContext ctx)
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        // Checks if score manager exists in the scene.
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("Score Manager does not exist in current scene!");
            return;
        }

        ScoreManager.Instance.ChangeScore(ScoreChange);
    }
}
