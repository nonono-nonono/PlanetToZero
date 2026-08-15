using System.Collections.Generic;
using UnityEngine;

public class SimpleListener : ListenerBase
{
    public override void Fire(EventContext ctx)
    {
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        foreach (ReactionBase reaction in Reactions)
        {
            reaction.Execute(ctx);
        }
    }
}
