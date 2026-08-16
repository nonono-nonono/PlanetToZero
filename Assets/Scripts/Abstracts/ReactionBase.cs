using UnityEngine;

// Base class for reactions. Execute is ran with event context as an arg by listeners. Reactions can choose to use/not use event context.
public abstract class ReactionBase : MonoBehaviour
{
    public abstract void Execute(EventContext ctx);
}
