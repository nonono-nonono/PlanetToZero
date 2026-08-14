using UnityEngine;

// Shoots a bullet towards mouse position
public class MouseBulletSummonReaction : ReactionBase
{
    public Team TargetTeam;
    public float Speed;
    public float BulletDuration;
    public GameObject AttackOrigin;
    public GameObject BulletPrefab;

    public override void Execute(EventContext ctx)
    {
        if (ctx is ClickContext clickContext)
        {
            Vector2 direction = (clickContext.MousePos - (Vector2)transform.position).normalized;
            
            GameObject obj = BulletPoolManager.Instance.FetchBullet(BulletPrefab);
            obj.transform.position = AttackOrigin.transform.position;

            Bullet newBullet = obj.GetComponent<Bullet>();

            newBullet.Shoot(Speed, direction, BulletDuration, TargetTeam);
        }
    }
}
