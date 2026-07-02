using System.Collections.Generic;
using UnityEngine;

public class HealthLifecycle : MonoBehaviour, IListenable, IAttackable
{
    [field: SerializeField] public float Max {get; private set;}
    [field: SerializeField] public float Current {get; private set;}
    [field: SerializeField] public List<ListenerBase> ListenerList {get; private set;}

    private AttackManager _attackManager;

    void Awake()
    {
        if (Current > Max || Current <= 0)
        {
            Current = Max;
        }
    }

    public void GetAttackManagerReference(AttackManager attackManager)
    {
        _attackManager = attackManager;
    }

    public void Register(ListenerBase listener)
    {
        ListenerList.Add(listener);
    }

    public void Deregister(ListenerBase listener)
    {
        ListenerList.Remove(listener);
    }

    public float TakeDamage(float amount)
    {
        float remainder = 0;

        if (amount > Current)
        {
            remainder = amount - Current;
        }

        Current = Mathf.Max(0, Current - amount);

        if (Current <= 0)
        {
            foreach(ListenerBase listener in ListenerList)
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

