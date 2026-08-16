using UnityEngine;

// Deletes a game object.
public class DeleteReaction : ReactionBase
{
    [SerializeField] private GameObject _target;
    
    public override void Execute(EventContext ctx)
    {
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;

        Destroy(_target);
    }
}
