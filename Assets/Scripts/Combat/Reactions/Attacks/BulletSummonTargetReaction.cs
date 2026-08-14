using UnityEngine;

public class BulletSummonReaction : ReactionBase
{
    public Transform TargetTransform;
    public Team TargetTeam;
    public float Speed;
    public float BulletDuration;
    public GameObject AttackOrigin;
    public GameObject BulletPrefab;

    public override void Execute(EventContext ctx)
    {
        Vector3 bulletSpawnPos = AttackOrigin.transform.position;
        Vector2 direction = (TargetTransform.position - AttackOrigin.transform.position).normalized;
        GameObject obj = BulletPoolManager.Instance.FetchBullet(BulletPrefab);

        obj.transform.position = AttackOrigin.transform.position;

        Bullet newBullet = obj.GetComponent<Bullet>();

        newBullet.Shoot(Speed, direction, BulletDuration, TargetTeam);
    }
}
