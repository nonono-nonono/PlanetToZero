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
