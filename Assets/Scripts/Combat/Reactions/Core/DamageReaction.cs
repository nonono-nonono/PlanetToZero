using UnityEngine;

public class DamageReaction : ReactionBase
{
    public AttackTypes AttackType;
    public Team TargetTeam;
    public float Damage;

    public override void Execute(EventContext ctx)
    {
        if (ctx is IDamageContext damageContext)
        {
            damageContext.AttackManager.DealDamageDefault(AttackType, TargetTeam, Damage);
        }
    }
}
