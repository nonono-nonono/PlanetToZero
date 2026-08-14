using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletContext : EventContext, IDamageContext
{
    public AttackManager AttackManager {get;}

    public BulletContext(AttackManager hit)
    {
        AttackManager = hit;
    }
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PooledBullet))]
public class Bullet : MonoBehaviour, IListenable
{
    [field: SerializeField] private List<ListenerBase> _listenerList;
    private Rigidbody2D _rb;
    private Team _targetTeam = Team.None;
    private Coroutine _moveRoutine;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Collider2D _cd = GetComponent<Collider2D>();
        _cd.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackManager attackManager = collision.gameObject.GetComponent<AttackManager>();

        if (attackManager && attackManager.Team == _targetTeam)
        {
            if (_moveRoutine != null) StopCoroutine(_moveRoutine);

            foreach (ListenerBase listener in _listenerList)
            {
                listener.Fire(new BulletContext(attackManager));
            }

            BulletPoolManager.Instance.ReturnBullet(gameObject);
        }
    }

    public void Shoot(float speed, Vector2 direction, float duration, Team targetTeam)
    {
        _targetTeam = targetTeam;
        _moveRoutine = StartCoroutine(MoveBullet(speed, direction, duration));
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
