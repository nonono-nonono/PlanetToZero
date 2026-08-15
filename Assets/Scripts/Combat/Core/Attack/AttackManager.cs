using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Team
{
    None,
    Player,
    Enemy
}

[RequireComponent(typeof(Collider2D))]
public class AttackManager : MonoBehaviour
{
    public Team Team;
    // Work around for Unity limitation on showing List<Interfaces> in Inspector
    public List<MonoBehaviour> AttackableObjs;
    private List<IAttackable> _attackables;
    
    void Awake()
    {
       _attackables = AttackableObjs
        .Where(a => a is IAttackable)
        .Cast<IAttackable>()
        .ToList();
    }

    void Start()
    {
        foreach (IAttackable attackable in _attackables)
        {
            attackable.GetAttackManagerReference(this);
        }
    }
    
    public void DealDamageDefault(AttackTypes attackType, Team targetTeam, float amount)
    {
        if (targetTeam != Team) return;

        List<IAttackable> targets = _attackables.OrderByDescending(a => AttackablePriorities.GetPriority(attackType, a)).ToList();

        float remainder = amount;

        foreach (IAttackable target in targets)
        {
            if (remainder <= 0f) break;

            remainder = target.TakeDamage(remainder);
        }
    }

    public void RegisterAttackable(IAttackable attackable)
    {
        _attackables.Add(attackable);
        attackable.GetAttackManagerReference(this);
    }

    public void DeregisterAttackable(IAttackable attackable)
    {
        _attackables.Remove(attackable);
    }
}
