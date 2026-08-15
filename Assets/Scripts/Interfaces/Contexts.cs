using UnityEngine;

public interface IBarContext
{
    float Current {get;}
    float Max {get;}
    float Amount {get;}
}

public interface IDamageContext
{
    AttackManager AttackManager {get;}
}

public interface IConditionalContext
{
    bool ConditionMet {get;}
}

public interface IInitializeContext
{
    bool IsInitializing {get;}
}

public interface IPositionContext
{
    Vector2 TargetPosition {get;}
}
