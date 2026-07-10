using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Team
{
    Player,
    Enemy
}

public class AttackManager : MonoBehaviour
{
    // Work around for Unity limitation on showing List<Interfaces> in Inspector
    public List<MonoBehaviour> AttackableObjs;
    private List<IAttackable> _attackables;
    [SerializeField] private Team _team;
    
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

    void OnEnable()
    {
        AttackManagerRegistry.Managers.Add(this);
    }

    void OnDisable()
    {
        AttackManagerRegistry.Managers.Remove(this);
    }


    public void DealDamageDefault(AttackTypes attackType, Team targetTeam, float amount)
    {
        if (targetTeam != _team) return;

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
