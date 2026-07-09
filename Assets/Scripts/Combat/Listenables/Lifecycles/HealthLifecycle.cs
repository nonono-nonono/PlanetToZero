using System.Collections.Generic;
using UnityEngine;

public enum HealthEvent
{
    OnDamage,
    OnHeal,
    OnDeath
}

public class HealthDamageContext : EventContext
{
    public float Amount;
    public HealthDamageContext(float amount) => Amount = amount;
}

public class HealthHealContext : EventContext
{
    public float Amount;
    public HealthHealContext(float amount) => Amount = amount;
}

public class HealthLifecycle : MonoBehaviour, IDynamicListenable<HealthEvent>, IAttackable
{
    [field: Header("Health Fields")]
    [field: SerializeField, Min(1)] public float Max {get; private set;}
    [field: SerializeField, Min(1)] public float Current {get; private set;}

    [field: Header("Event Types")]
    [field: SerializeField] public List<ListenerBase> OnDamageList {get; private set;}
    [field: SerializeField] public List<ListenerBase> OnHealList {get; private set;}
    [field: SerializeField] public List<ListenerBase> OnDeathList {get; private set;}

    private readonly ListenerRegistry<HealthEvent>_listenerRegistry = new();
    private AttackManager _attackManager;

    void Awake()
    {
        if (Current > Max || Current <= 0)
        {
            Current = Max;
        }

        InitializeRegistry();
    }

    private void InitializeRegistry()
    {
        foreach (ListenerBase listener in OnDamageList)
        {
            _listenerRegistry.Register(HealthEvent.OnDamage, listener);
        }

        foreach (ListenerBase listener in OnHealList)
        {
            _listenerRegistry.Register(HealthEvent.OnHeal, listener);
        }

        foreach (ListenerBase listener in OnDeathList)
        {
            _listenerRegistry.Register(HealthEvent.OnDeath, listener);
        }
    }

    public void GetAttackManagerReference(AttackManager attackManager)
    {
        _attackManager = attackManager;
    }

    public void Register(HealthEvent eventType, ListenerBase listener)
    {
        _listenerRegistry.Register(eventType, listener);
    }

    public void Deregister(HealthEvent eventType, ListenerBase listener)
    {
        _listenerRegistry.Deregister(eventType, listener);
    }

    public float TakeDamage(float amount)
    {
        float remainder = 0;
        float damageTaken;

        if (amount > Current)
        {
            damageTaken = Current;
            remainder = amount - Current;
        }
        else
        {
            damageTaken = amount;
        }

        Current = Mathf.Max(0, Current - amount);

        foreach(ListenerBase listener in _listenerRegistry.FetchListenersByType(HealthEvent.OnDamage))
        {
            listener.Fire(new HealthDamageContext(damageTaken));
        }

        Debug.Log(Current);

        if (Current <= 0)
        {
            foreach(ListenerBase listener in _listenerRegistry.FetchListenersByType(HealthEvent.OnDeath))
            {
                listener.Fire(null);
            }

            _attackManager.DeregisterAttackable(this);
        }

        return remainder;
    }

    public void Heal(float amount)
    {
        Current = Mathf.Min(Max, Current + amount);
    }
}

