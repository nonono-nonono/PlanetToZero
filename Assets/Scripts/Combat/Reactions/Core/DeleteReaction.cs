using UnityEngine;

public class DeleteReaction : ReactionBase
{
    [SerializeField] private GameObject _target;

    public override void Execute(EventContext ctx)
    {
        Destroy(_target);
    }
}
