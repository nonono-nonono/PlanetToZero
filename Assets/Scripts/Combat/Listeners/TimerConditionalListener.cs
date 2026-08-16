using UnityEngine;

// Repeatedly executes all its reactions on a timer provided a condition is met. 
public class TimerConditionalListener : ListenerBase
{
    // Listener executes at a different time from when listenable fires. It needs a source of context.
    [SerializeField] private MonoBehaviour _contextSourceBehavior;
    [SerializeField] private float _interval;
    private float _elapsed;
    private bool _canFireReactions;
    private IContextPullable _contextSource => _contextSourceBehavior as IContextPullable;

    public override void Fire(EventContext ctx)
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;

        // When fired, set whether the condition is met to given IConditionalContext.ConditionMet
        if (ctx is IConditionalContext conditionalContext)
        {
            _canFireReactions = conditionalContext.ConditionMet;
        }
    }

    void Update()
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        // Increases elapsed by deltaTime until it hits interval.
        if (_elapsed < _interval)
        {
            _elapsed += Time.deltaTime;
            return;
        }
        
        // If condition is not met ignore.
        if (!_canFireReactions) return;

        _elapsed = 0f;
        
        // Get Context (? null conditional operator checks if context source is null)
        EventContext ctx = _contextSource?.GrabContext();

        // Fire all reactions with given context.
        foreach (ReactionBase reaction in Reactions)
        {
            reaction.Execute(ctx);
        }
    }
}
