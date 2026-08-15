using System.Collections;
using System.Linq;
using UnityEngine;

public class RectangularAreaAttackReaction : ReactionBase
{
    public GameObject AttackIndicatorPrefab;
    public GameObject AttackOrigin;
    public Vector2 AttackSize;
    public Team TargetTeam;
    public float Damage;
    public float WindUpTime;
    private GameObject _currentAttackIndicator;

    public override void Execute(EventContext ctx)
    {
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        if (ctx is IPositionContext directionCtx)
        {
            StartCoroutine(AttackRoutine(directionCtx));
        }
    }

    private IEnumerator AttackRoutine(IPositionContext directionCtx)
    {
        Vector2 direction = (directionCtx.TargetPosition - (Vector2)AttackOrigin.transform.position).normalized;
        GameObject newAttackIndicator = Instantiate(AttackIndicatorPrefab);

        _currentAttackIndicator = newAttackIndicator;

        newAttackIndicator.transform.SetPositionAndRotation(AttackOrigin.transform.position, Quaternion.LookRotation(Vector3.forward, direction));
        newAttackIndicator.transform.localScale = new Vector3(AttackSize.x, AttackSize.y, 1);

        yield return Wait(WindUpTime);

        if (GameManager.Instance.GetGameState() != GameState.Playing)
        {
            yield break;
        }

        // Reverse of Cos(x), Sin(y) formula
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        Vector2 centerOffset = new Vector2(0f, AttackSize.y / 2f);

        // Attack indicator only ever rotates about Z Axis
        Vector2 rotatedOffset = Quaternion.Euler(0, 0, angle) * centerOffset;
        Vector2 boxCenter = (Vector2)AttackOrigin.transform.position + rotatedOffset;

        AttackManager[] validTargets = Physics2D.OverlapBoxAll(boxCenter, AttackSize, angle)
            .Select(gameObj => gameObj.GetComponent<AttackManager>())
            .Where(manager => manager != null)
            .ToArray();

        foreach (AttackManager manager in validTargets)
        {
            manager.DealDamageDefault(AttackTypes.Basic, TargetTeam, Damage);
        }

        Destroy(newAttackIndicator);
        _currentAttackIndicator = null;
    }

    private IEnumerator Wait(float time)
    {
        float elapsed = 0f;

        while (elapsed < time)
        {
            if (GameManager.Instance.GetGameState() != GameState.Playing)
            {
                if (_currentAttackIndicator != null)
                {
                    Destroy(_currentAttackIndicator);
                    _currentAttackIndicator = null;
                }
                
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();

        if (_currentAttackIndicator != null)
        {
            Destroy(_currentAttackIndicator);
            _currentAttackIndicator = null;
        }
    }
}
