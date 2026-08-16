using System.Collections.Generic;
using UnityEngine;

// Health event to define what event listeners are attaching to.
public enum HealthEvent
{
    OnChanged,
    OnDeath
}

// Context given to all listeners. Implements IBarContext and IInitializeContext
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

// Healthlifecycle listenable which fires everytime health is changed and when health reaches 0. Can be registered into an Attack Manager.
// IDynamicListenable takes HealthEvent in, when registering to this listenable. You can only use items defined under the enum.
public class HealthLifecycle : MonoBehaviour, IDynamicListenable<HealthEvent>, IAttackable
{
    [Header("Health Fields")]
    [SerializeField, Min(1)] private float _max;
    [SerializeField, Min(1)] public float _current;

    [Header("Event Types")]
    public List<ListenerBase> OnChangedList;
    public List<ListenerBase> OnDeathList;

    private ListenerRegistry<HealthEvent> _listenerRegistry = new();
    private AttackManager _attackManager;
    
    void Awake()
    {
        // Sets current health to whatever max health is if > max health or current health < 0
        if (_current > _max || _current <= 0)
        {
            _current = _max;
        }

        // Registers listeners from OnChanged and OnDeath to the listener registry.
        InitializeRegistry();
    }


    void Start()
    {   
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;

        // Fires all listeners connected to onChanged event.
        // Context contains: health changed amount, current health, max health and whether it is just initializing the health or not.
        foreach (ListenerBase listenerBase in _listenerRegistry.FetchListenersByType(HealthEvent.OnChanged))
        {
            listenerBase.Fire(new HealthChangedContext(_current, _current, _max, true));
        }
    }

    // IAttackable Implementation
    public void GetAttackManagerReference(AttackManager attackManager)
    {
        _attackManager = attackManager;
    }

    // IDynamicListenable Implementation
    public void Register(HealthEvent eventType, ListenerBase listener)
    {
        _listenerRegistry.Register(eventType, listener);
    }

    // IDynamicListenable Implementation
    public void Deregister(HealthEvent eventType, ListenerBase listener)
    {
        _listenerRegistry.Deregister(eventType, listener);
    }

    // IDynamicListenable Implementation
    public ListenerBase[] FetchListenersByType(HealthEvent eventType)
    {
        return _listenerRegistry.FetchListenersByType(eventType);
    }

    // Deals damage to the health lifecycle. Returns remainder damage to deal to other attackables.
    public float TakeDamage(float amount)
    {
        // Returns 0 if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return 0f;

        // Ensure amount is > 0.
        amount = Mathf.Max(0, amount);

        // Calculates damage taken and makes sure damage taken does not exceed remaining health.
        // Finds remainder damage.
        float damageTaken = Mathf.Min(amount, _current);
        float remainder = amount - damageTaken;
 
        _current -= damageTaken;

        // Fire all listeners connected to OnChanged even with HealthChangedContext
        foreach(ListenerBase listener in _listenerRegistry.FetchListenersByType(HealthEvent.OnChanged))
        {
            listener.Fire(new HealthChangedContext(-damageTaken, _current, _max, false));
        }
        
        // Checks if current health reaches 0. If so, fires all listeners connected to OnDeath event with no event context.
        if (_current <= 0)
        {
            foreach(ListenerBase listener in _listenerRegistry.FetchListenersByType(HealthEvent.OnDeath))
            {
                listener.Fire(null);
            }

            // Deregisters this attackable from attack manager after dying.
            _attackManager.DeregisterAttackable(this);
        }

        // Return remainder damage to deal to other attackbles.
        return remainder;
    }

    // Heals health to the health lifecycle. 
    public void Heal(float amount)
    {   
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        // Ensures amount > 0;
        amount = Mathf.Max(0, amount);
        
        // Calculates healing done, makes sure healing done is not more than health difference between max and current (health needed from current to reach max)
        float healingDone = Mathf.Min(amount, _max - _current);

        _current += healingDone;

        // Fire all listeners connected to OnChanged even with HealthChangedContext
        foreach(ListenerBase listener in _listenerRegistry.FetchListenersByType(HealthEvent.OnChanged))
        {
            listener.Fire(new HealthChangedContext(healingDone, _current, _max, false));
        }
    }

    // Registers listeners from OnChanged and OnDeath to the listener registry.
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

