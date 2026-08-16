using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// All possible teams for attack managers.
public enum Team
{
    None,
    Player,
    Enemy
}

// Handles all incoming attacks.
[RequireComponent(typeof(Collider2D))]
public class AttackManager : MonoBehaviour
{
    public Team Team;
    // Work around for Unity limitation on showing List<Interfaces> in Inspector.
    public List<MonoBehaviour> AttackableObjs;
    private List<IAttackable> _attackables;
    
    // Sets _attackables to a list of IAttackable component based of AttackbleObjs. (Ignores every non IAttackble components).
    void Awake()
    {
       _attackables = AttackableObjs
        .Where(a => a is IAttackable)
        .Cast<IAttackable>()
        .ToList();
    }

    // Passes reference to itself for every IAttackable.
    void Start()
    {
        foreach (IAttackable attackable in _attackables)
        {
            attackable.GetAttackManagerReference(this);
        }
    }
    
    // Looks at all IAttackables registered to this manager and given attack type and damages them based on IAttackable Priorities for that attack type.
    public void DealDamageDefault(AttackTypes attackType, Team targetTeam, float amount)
    {
        // Ignores the attack if attack manager is not the targetted team.
        if (targetTeam != Team) return;

        // Gets a list of highest priority to lowest priority of IAttackables registered to this attack maanager. 
        List<IAttackable> targets = _attackables.OrderByDescending(a => AttackablePriorities.GetPriority(attackType, a)).ToList();

        float remainder = amount;

        // Attacks the highest priority IAttackable, if the amount of damage dealt exceeds the remaining "health" for that IAttackable, transfers the damage to the next in line IAttackable.
        foreach (IAttackable target in targets)
        {
            if (remainder <= 0f) break;

            remainder = target.TakeDamage(remainder);
        }
    }

    // Registers an IAttackable at runtime.
    public void RegisterAttackable(IAttackable attackable)
    {
        _attackables.Add(attackable);
        attackable.GetAttackManagerReference(this);
    }

    // Deregisters an IAttackable at runtime.
    public void DeregisterAttackable(IAttackable attackable)
    {
        _attackables.Remove(attackable);
    }
}
