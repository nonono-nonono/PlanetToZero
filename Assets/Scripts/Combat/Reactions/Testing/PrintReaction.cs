using UnityEngine;

public class PrintReaction : ReactionBase
{
    public override void Execute(EventContext ctx)
    {
        Debug.Log("Printed!");
    }
}
