using UnityEngine;

public abstract class ReactionBase : MonoBehaviour
{
    public abstract void Execute(EventContext ctx);
}
