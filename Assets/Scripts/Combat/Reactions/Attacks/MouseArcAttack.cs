using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Attacks in an arc shape using mouse position, can implement a default way if mouse position is not given later.
// Multi-hits by default
public class MouseArcAttack : ReactionBase
{
    public Team TargetTeam;
    public float Range;
    public float Damage;
    [Range(0f, 360f)] public float ArcAngle;
    public GameObject AttackOrigin;

    public override void Execute(EventContext ctx)
    {
        if (ctx is ClickContext clickContext)
        {
            Vector2 direction = (clickContext.MousePos - (Vector2)transform.position).normalized;

            // Filter to check for attack managers within distance and within angle.
            List<AttackManager> validTargets = AttackManagerRegistry.Managers
                .Where(manager => CalculateDistance(AttackOrigin.transform, manager.transform) <= Range)
                .Where(manager => CalculateAngle(direction, ((Vector2)manager.transform.position - (Vector2)AttackOrigin.transform.position).normalized) <= ArcAngle / 2f)
                .ToList();
            
            foreach (AttackManager attackManager in validTargets)
            {
                attackManager.DealDamageDefault(AttackTypes.Basic, TargetTeam, Damage);
            }
        }
    }

    private float CalculateDistance(Transform origin, Transform target)
    {
        return (origin.position - target.position).magnitude;
    }

    private float CalculateAngle(Vector2 direction, Vector2 targetDirection)
    {
        return Vector2.Angle(direction, targetDirection);
    }
}
