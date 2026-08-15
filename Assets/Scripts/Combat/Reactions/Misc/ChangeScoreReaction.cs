using UnityEngine;

public class ChangeScoreReaction : ReactionBase
{
    public float ScoreChange;
    public override void Execute(EventContext ctx)
    {
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("Score Manager does not exist in current scene!");
            return;
        }

        ScoreManager.Instance.ChangeScore(ScoreChange);
    }
}
