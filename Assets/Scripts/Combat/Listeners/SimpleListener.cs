using System.Collections.Generic;
using UnityEngine;

public class SimpleListener : ListenerBase
{
    public override void Fire(EventContext ctx)
    {
        foreach (ReactionBase reaction in Reactions)
        {
            reaction.Execute(ctx);
        }
    }
}
