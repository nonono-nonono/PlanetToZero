using System.Collections.Generic;

public enum AttackTypes
{
    Basic
}

public static class AttackablePriorities
{
    // Higher = checked first, Lower = checked later
    public static readonly Dictionary<AttackTypes, Dictionary<System.Type, int>> Priorities = new()
    {
        {
            AttackTypes.Basic,
            new Dictionary<System.Type, int>()
            {
                {typeof(HealthLifecycle), 1}
            }
        }
    };

    public static int GetPriority(AttackTypes attackType, IAttackable attackable)
    {
        return Priorities[attackType].TryGetValue(attackable.GetType(), out int priority) ? priority : 0;
    }
}
