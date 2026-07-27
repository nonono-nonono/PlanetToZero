using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletContext : EventContext
{
    public AttackManager Hit;
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PooledBullet))]
public class Bullet : MonoBehaviour, IListenable
{
    [field: SerializeField] private List<ListenerBase> _listenerList;
    private Rigidbody2D _rb;
    private Collider2D _cd;
    private Team _targetTeam = Team.None;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cd = GetComponent<Collider2D>();
    }
    public void Shoot(float speed, Vector2 direction, float duration, Team targetTeam)
    {
        _targetTeam = targetTeam;
        StartCoroutine(MoveBullet(speed, direction, duration));
    }
    public void Register(ListenerBase listener)
    {
        _listenerList.Add(listener);
    }

    public void Deregister(ListenerBase listener)
    {
        _listenerList.Remove(listener);
    }

    public ListenerBase[] FetchListeners()
    {
        return _listenerList.ToArray();
    }
    private IEnumerator MoveBullet(float speed, Vector2 direction, float duration)
    {
        _rb.linearVelocity = direction * speed;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        _rb.linearVelocity = Vector2.zero;
        _targetTeam = Team.None;
        BulletPoolManager.Instance.ReturnBullet(gameObject);
    }
}
