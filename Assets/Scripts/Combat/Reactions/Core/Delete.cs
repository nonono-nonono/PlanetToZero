using UnityEngine;

public class Delete : ReactionBase
{
    [SerializeField] private GameObject _target;

    public override void Execute(EventContext ctx)
    {
        Destroy(_target);
    }
}
