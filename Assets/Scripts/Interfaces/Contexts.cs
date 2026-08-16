using UnityEngine;

// All contexts here is to allow for listenables to implement them for their contexts. Listeners and reactions can choose whether they want to use them or not.
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