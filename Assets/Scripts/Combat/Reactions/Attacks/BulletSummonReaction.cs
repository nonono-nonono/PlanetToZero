using UnityEngine;

// Shoots a bullet towards mouse position
public class BulletSummonReaction : ReactionBase
{
    public Team TargetTeam;
    public float Speed;
    public float BulletDuration;
    public GameObject AttackOrigin;
    public GameObject BulletPrefab;

    public override void Execute(EventContext ctx)
    {
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
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
