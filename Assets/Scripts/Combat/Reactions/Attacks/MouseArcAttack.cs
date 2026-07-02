using System;
using UnityEngine;

public class MouseArcAttack : ReactionBase
{
    public float Range;
    [Range(0f, 360f)] public float ArcAngle;
    public override void Execute(EventContext ctx)
    {
        
    }
}
