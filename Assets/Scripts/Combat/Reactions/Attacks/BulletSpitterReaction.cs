using System;
using UnityEngine;

public class BulletSplitter : ReactionBase
{
    public Team TargetTeam;
    public float Speed;
    public float BulletDuration;
    public int BulletCount;
    public GameObject AttackOrigin;
    public GameObject BulletPrefab;

    public override void Execute(EventContext ctx)
    {
        if (ctx is IInitializeContext initializeCtx)
        {
            if (initializeCtx.IsInitializing)
            return;
        }

        float angleStep = 360f / BulletCount;
        Vector3 bulletSpawnPos = AttackOrigin.transform.position;

        for (int i = 0; i < BulletCount; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = ConvertToDirection(angle);

            GameObject obj = BulletPoolManager.Instance.FetchBullet(BulletPrefab);
            obj.transform.position = bulletSpawnPos;

            Bullet newBullet = obj.GetComponent<Bullet>();

            newBullet.Shoot(Speed, direction, BulletDuration, TargetTeam);
        }
    }

    private Vector2 ConvertToDirection(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;

        // Simple circle math and unity expects radians for Cos and Sin
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}

