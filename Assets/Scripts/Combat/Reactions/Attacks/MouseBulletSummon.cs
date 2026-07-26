using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// Shoots a bullet towards mouse position
public class MouseBulletSummon : ReactionBase
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
            GameObject newBullet = BulletPoolManager.Instance.FetchBullet(BulletPrefab);
            newBullet.transform.position = transform.position;
            StartCoroutine(MoveBullet(newBullet.GetComponent<Rigidbody2D>(), direction, Speed));
        }
    }

    private IEnumerator MoveBullet(Rigidbody2D rb, Vector2 direction, float speed)
    {
        rb.linearVelocity = direction * speed;

        yield return new WaitForSeconds(BulletDuration);

        BulletPoolManager.Instance.ReturnBullet(BulletPrefab, rb.gameObject);
    }
}
