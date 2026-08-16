using System;
using UnityEngine;

// Spits multiple bullets out from all directions.
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
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        // If event context given implements IInitializeContext, ignore if the event is fired just to initialize everything.
        if (ctx is IInitializeContext initializeCtx)
        {
            if (initializeCtx.IsInitializing)
            return;
        }

        float angleStep = 360f / BulletCount;
        Vector3 bulletSpawnPos = AttackOrigin.transform.position;

        // For each bullet, calculate the direction the bullet will travel, fetch a bullet from the pool, position the bullet at the attack origin and shoot the bullet.
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

    // Converts an angle to vector2
    private Vector2 ConvertToDirection(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;

        // Simple circle math and unity expects radians for Cos and Sin
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}

