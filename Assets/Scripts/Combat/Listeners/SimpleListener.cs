// Simple listener executes all reactions with given event context everytime it is fired.
public class SimpleListener : ListenerBase
{
    public override void Fire(EventContext ctx)
    {   
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        foreach (ReactionBase reaction in Reactions)
        {
            reaction.Execute(ctx);
        }
    }
}
