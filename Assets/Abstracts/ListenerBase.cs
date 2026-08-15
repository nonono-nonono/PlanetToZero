using System.Collections.Generic;
using UnityEngine;

public abstract class ListenerBase : MonoBehaviour
{
    [SerializeField] protected List<ReactionBase> Reactions;

    public ReactionBase[] FetchReactions()
    {
        return Reactions.ToArray();
    }
    
    public abstract void Fire(EventContext ctx);
}


