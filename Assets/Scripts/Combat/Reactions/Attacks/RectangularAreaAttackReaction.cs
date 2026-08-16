using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Creates a rectangular attack indicator and after a wind up time, deal damage to whatever is still in that area.
public class RectangularAreaAttackReaction : ReactionBase
{
    public GameObject AttackIndicatorPrefab;
    public GameObject AttackOrigin;
    public Vector2 AttackSize;
    public Team TargetTeam;
    public float Damage;
    public float WindUpTime;

    // Used to cleanup attack indicators if component/gameobject is destroyed.
    private List<GameObject> _currentAttackIndicator = new();

    public override void Execute(EventContext ctx)
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;

        // Starts attack routine if event context given implements IPositionContext.
        if (ctx is IPositionContext directionCtx)
        {
            StartCoroutine(AttackRoutine(directionCtx));
        }
    }

    // Creates an attack indicator rotated towards given position, after wind up time, destroy attack indicator and damage whatever is inside the area.
    private IEnumerator AttackRoutine(IPositionContext directionCtx)
    {
        // (Vector2)AttackOrigin this is explicit type conversion
        Vector2 direction = (directionCtx.TargetPosition - (Vector2)AttackOrigin.transform.position).normalized;
        GameObject newAttackIndicator = Instantiate(AttackIndicatorPrefab);

        // Add created attack indicator to list of attack indicators.
        _currentAttackIndicator.Add(newAttackIndicator);
        
        // Position attack indicator to attack origin. Rotate and resize attack indicator to given position.
        newAttackIndicator.transform.SetPositionAndRotation(AttackOrigin.transform.position, Quaternion.LookRotation(Vector3.forward, direction));
        newAttackIndicator.transform.localScale = new Vector3(AttackSize.x, AttackSize.y, 1);

        yield return Wait(WindUpTime);

        // Stops coroutine if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing)
        {
            yield break;
        }

        // Reverse of Cos(x), Sin(y) formula. Converts Vector2 to angle.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Amount of offset to the center of the attack indicator based on its size.
        Vector2 centerOffset = new(0f, AttackSize.y / 2f);

        // Rotate the offset and get the boxes center from attack origin using the rotated offset.
        Vector2 rotatedOffset = Quaternion.Euler(0, 0, angle) * centerOffset;
        Vector2 boxCenter = (Vector2)AttackOrigin.transform.position + rotatedOffset;

        // Draw an invisible box using the boxes center, the size and the angle.
        // Gets all colliders in the drawn box and gets a list of attack managers from the colliders.
        AttackManager[] validTargets = Physics2D.OverlapBoxAll(boxCenter, AttackSize, angle)
            .Select(gameObj => gameObj.GetComponent<AttackManager>())
            .Where(manager => manager != null)
            .ToArray();
        
        // Deals damage to all attack managers found.
        foreach (AttackManager manager in validTargets)
        {
            manager.DealDamageDefault(AttackTypes.Basic, TargetTeam, Damage);
        }

        // Remove attack indicator from the list and destroy it.
        _currentAttackIndicator.Remove(newAttackIndicator);
        Destroy(newAttackIndicator);
    }

    // Wait Coroutine which also checks if the game state is still playing.
    private IEnumerator Wait(float time)
    {
        float elapsed = 0f;

        while (elapsed < time)
        {
            // If game state isn't playing, destroy all still existing attack indicators and break out of this coroutine.
            if (GameManager.Instance.GetGameState() != GameState.Playing)
            {
                if (_currentAttackIndicator.Count > 0)
                {
                    foreach (GameObject attackIndicator in _currentAttackIndicator)
                    {
                        Destroy(attackIndicator);
                    }

                    _currentAttackIndicator.Clear();
                }
                
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    // Stop all attacking/wait coroutines and destroy all existing attack indicators from this component when this component is destroyed.
    void OnDestroy()
    {
        StopAllCoroutines();

        if (_currentAttackIndicator.Count > 0)
        {
            foreach (GameObject attackIndicator in _currentAttackIndicator)
            {
                Destroy(attackIndicator);
            }

            _currentAttackIndicator.Clear();
        }
    }
}
