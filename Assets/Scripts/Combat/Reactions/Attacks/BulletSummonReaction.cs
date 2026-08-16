using UnityEngine;

// Shoots a bullet towards a direction
public class BulletSummonReaction : ReactionBase
{
    public Team TargetTeam;
    public float Speed;
    public float BulletDuration;
    public GameObject AttackOrigin;
    public GameObject BulletPrefab;

    public override void Execute(EventContext ctx)
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        // If event context implements IPositionContext, gets the direction to go from attack origin to given position in the context.
        // Fetches a bullet from the bullet pool, position it in attack origin and shoots the bullet.
        if (ctx is IPositionContext directionCtx)
        {
            Vector2 direction = (directionCtx.TargetPosition - (Vector2)AttackOrigin.transform.position).normalized;
            
            GameObject obj = BulletPoolManager.Instance.FetchBullet(BulletPrefab);
            obj.transform.position = AttackOrigin.transform.position;

            Bullet newBullet = obj.GetComponent<Bullet>();

            newBullet.Shoot(Speed, direction, BulletDuration, TargetTeam);
        }
    }
}
