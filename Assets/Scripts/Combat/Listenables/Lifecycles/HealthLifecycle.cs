using System.Collections.Generic;
using UnityEngine;

public enum HealthEvent
{
    OnChanged,
    OnDeath
}

public class HealthChangedContext : EventContext, IBarContext, IInitializeContext
{
    public float Amount {get;}
    public float Current {get;}
    public float Max {get;}
    public bool IsInitializing {get;}

    public HealthChangedContext(float amount, float current, float max, bool isInitializing)
    {
        Amount = amount;
        Current = current;
        Max = max;
        IsInitializing = isInitializing;
    }
}

public class HealthLifecycle : MonoBehaviour, IDynamicListenable<HealthEvent>, IAttackable
{
    [field: Header("Health Fields")]
    [field: SerializeField, Min(1)] public float Max {get; private set;}
    [field: SerializeField, Min(1)] public float Current {get; private set;}

    [field: Header("Event Types")]
    public List<ListenerBase> OnChangedList;
    public List<ListenerBase> OnDeathList;

    private readonly ListenerRegistry<HealthEvent> _listenerRegistry = new();
    private AttackManager _attackManager;

    void Awake()
    {
        if (Current > Max || Current <= 0)
        {
            Current = Max;
        }

        InitializeRegistry();
    }

    void Start()
    {
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;

        foreach (ListenerBase listenerBase in _listenerRegistry.FetchListenersByType(HealthEvent.OnChanged))
        {
            listenerBase.Fire(new HealthChangedContext(Current, Current, Max, true));
        }
    }

    [ContextMenu("Take Damage")]
    void Test()
    {
        TakeDamage(10);
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

    public ListenerBase[] FetchListenersByType(HealthEvent eventType)
    {
        return _listenerRegistry.FetchListenersByType(eventType);
    }

    public float TakeDamage(float amount)
    {
        if (GameManager.Instance.GetGameState() != GameState.Playing) return 0f;

        amount = Mathf.Max(0, amount);

        float damageTaken = Mathf.Min(amount, Current);
        float remainder = amount - damageTaken;
 
        Current -= damageTaken;

        foreach(ListenerBase listener in _listenerRegistry.FetchListenersByType(HealthEvent.OnChanged))
        {
            listener.Fire(new HealthChangedContext(-damageTaken, Current, Max, false));
        }

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
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        amount = Mathf.Max(0, amount);
        
        float healingDone = Mathf.Min(amount, Max - Current);

        Current += healingDone;

        foreach(ListenerBase listener in _listenerRegistry.FetchListenersByType(HealthEvent.OnChanged))
        {
            listener.Fire(new HealthChangedContext(healingDone, Current, Max, false));
        }
    }

    private void InitializeRegistry()
    {
        foreach (ListenerBase listener in OnChangedList)
        {
            _listenerRegistry.Register(HealthEvent.OnChanged, listener); 
        }

        foreach (ListenerBase listener in OnDeathList)
        {
            _listenerRegistry.Register(HealthEvent.OnDeath, listener);
        }
    }
}

