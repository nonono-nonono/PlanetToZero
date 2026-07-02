using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public List<MonoBehaviour> AttackableObjs;
    private List<IAttackable> _attackables;
    

    void Awake()
    {
       _attackables = AttackableObjs.Where(a => a is IAttackable).Cast<IAttackable>().ToList();
    }

    void Start()
    {
        foreach (IAttackable attackable in _attackables)
        {
            attackable.GetAttackManagerReference(this);
        }

        DealDamageDefault(AttackTypes.Basic, 100f);
    }

    public void DealDamageDefault(AttackTypes attackType, float amount)
    {
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
