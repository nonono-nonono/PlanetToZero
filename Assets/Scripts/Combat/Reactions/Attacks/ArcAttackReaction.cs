using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcAttackReaction : ReactionBase
{
    public Team TargetTeam;
    public float Range;
    public float Damage;
    [Range(0f, 360f)] public float ArcAngle;
    public GameObject AttackOrigin;

    public override void Execute(EventContext ctx)
    {
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        if (ctx is IPositionContext clickContext)
        {
            Vector2 direction = (clickContext.TargetPosition - (Vector2)transform.position).normalized;

            // Filter to check for attack managers within distance and within angle.
            AttackManager[] validTargets= Physics2D.OverlapCircleAll(AttackOrigin.transform.position, Range)
                .Where(gameObj => CalculateAngle(direction, ((Vector2)gameObj.transform.position - (Vector2)AttackOrigin.transform.position).normalized) <= ArcAngle / 2f)
                .Select(gameObj => gameObj.GetComponent<AttackManager>())
                .Where(manager => manager != null)
                .ToArray();
            
            foreach (AttackManager attackManager in validTargets)
            {
                attackManager.DealDamageDefault(AttackTypes.Basic, TargetTeam, Damage);
            }
        }
    }
    
    private float CalculateAngle(Vector2 direction, Vector2 targetDirection)
    {
        return Vector2.Angle(direction, targetDirection);
    }
}
