// Deals damage to a target based on given context.
public class DamageReaction : ReactionBase
{
    public AttackTypes AttackType;
    public Team TargetTeam;
    public float Damage;

    public override void Execute(EventContext ctx)
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        if (ctx is IDamageContext damageContext)
        {
            damageContext.AttackManager.DealDamageDefault(AttackType, TargetTeam, Damage);
        }
    }
}
