using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Context given to listeners when bullet hits something. Implements IDamageContext.
public class BulletContext : EventContext, IDamageContext
{
    public AttackManager AttackManager {get;}

    public BulletContext(AttackManager hit)
    {
        AttackManager = hit;
    }
}

// Bullet listenable. Listeners can plug into when the bullet hits a valid target.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PooledBullet))]
public class Bullet : MonoBehaviour, IListenable
{
    [SerializeField] private List<ListenerBase> _listenerList;
    private Rigidbody2D _rb;
    private Team _targetTeam = Team.None;
    private Coroutine _moveRoutine;

    // Makes sure collider2d is set to isTrigger (rigidbodies go through it rather than hitting it)
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Collider2D _cd = GetComponent<Collider2D>();
        _cd.isTrigger = true;
    }

    // Fires when something enter the bullet's collider.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;

        // Tries to get attack manager on the game object that entered the bullet's collider.
        AttackManager attackManager = collision.gameObject.GetComponent<AttackManager>();

        // Checks if attack manager exists and attack manager is on the team targetted by the bullet.
        if (attackManager && attackManager.Team == _targetTeam)
        {
            // Stops the coroutine moving the bullet
            if (_moveRoutine != null) StopCoroutine(_moveRoutine);

            // Fires all listeners attached to it with new bullet context containing the attack manager that was obtained from the game object.
            foreach (ListenerBase listener in _listenerList)
            {
                listener.Fire(new BulletContext(attackManager));
            }

            // Returns bullet to bullet pool.
            BulletPoolManager.Instance.ReturnBullet(gameObject);
        }
    }

    // Starts and stores a coroutine that moves the bullet forward every frame. 
    public void Shoot(float speed, Vector2 direction, float duration, Team targetTeam)
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        _targetTeam = targetTeam;
        _moveRoutine = StartCoroutine(MoveBullet(speed, direction, duration));
    }

    // Part of IListenable inteface implementation.
    public void Register(ListenerBase listener)
    {
        _listenerList.Add(listener);
    }

    // Part of IListenable inteface implementation.
    public void Deregister(ListenerBase listener)
    {
        _listenerList.Remove(listener);
    }

    // Part of IListenable inteface implementation.
    public ListenerBase[] FetchListeners()
    {
        return _listenerList.ToArray();
    }

    // Coroutine for MoveBullet, moves in a given direction at a given speed and returns bullet to bullet pool after a given duration.
    private IEnumerator MoveBullet(float speed, Vector2 direction, float duration)
    {
        // Unity automatically updates rigidbody position based on linear velocity. (_rb.position + linearvelo * Time.deltaTime)
        _rb.linearVelocity = direction * speed;

        float elapsed = 0f;

        // Keeps incrementing elapsed every frame by Time.deltaTime until it hits duration.
        while (elapsed < duration)
        {
            // Returns bullet to bullet pool if game state isn't playing. Yield break is equivalent to return in coroutines.
            if (GameManager.Instance.GetGameState() != GameState.Playing)
            {
                BulletPoolManager.Instance.ReturnBullet(gameObject);
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Stops moving the bullet, make sure it isn't able to target any attack manager and return to bullet pool.
        _rb.linearVelocity = Vector2.zero;
        _targetTeam = Team.None;
        BulletPoolManager.Instance.ReturnBullet(gameObject);
    }
}
