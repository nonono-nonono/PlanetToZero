using UnityEngine;

public class SimpleListener : ListenerBase
{
    public override void Fire()
    {
        foreach (ReactionBase reaction in Reactions)
        {
            reaction.Execute();
        }
    }
}
