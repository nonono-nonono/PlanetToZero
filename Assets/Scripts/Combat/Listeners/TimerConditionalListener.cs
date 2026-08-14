using System.Timers;
using UnityEngine;

public class TimerConditionalListener : ListenerBase
{
    [SerializeField] private float _interval;
    private float _elapsed;
    private bool _canFireReactions;

    public override void Fire(EventContext ctx)
    {
        if (ctx is IConditionalContext conditionalContext)
        {
            _canFireReactions = conditionalContext.ConditionMet;
        }
    }

    void Update()
    {
        if (_elapsed < _interval)
        {
            _elapsed += Time.deltaTime;
            return;
        }
        
        if (!_canFireReactions) return;

        _elapsed = 0f;

        foreach (ReactionBase reaction in Reactions)
        {
            reaction.Execute(null);
        }
    }
}
