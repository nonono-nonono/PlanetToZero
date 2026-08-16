using System.Collections.Generic;
using UnityEngine;

// Base class for listener, holds a list of reactions. Listeners run fire which fires all its reactions. Listeners can choose to use/not use event context when firing reactions.
public abstract class ListenerBase : MonoBehaviour
{
    [SerializeField] protected List<ReactionBase> Reactions;

    public ReactionBase[] FetchReactions()
    {
        return Reactions.ToArray();
    }
    
    public abstract void Fire(EventContext ctx);
}


