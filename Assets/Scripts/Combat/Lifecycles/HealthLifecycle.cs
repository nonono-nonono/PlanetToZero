using System.Collections.Generic;
using UnityEngine;
public class HealthLifecycle : MonoBehaviour, IListenable
{
    [field: SerializeField] public float Max {get; private set;}
    [field: SerializeField] public float Current {get; private set;}

    public List<ListenerBase> ListenerList;
    private List<ListenerBase> _listenerList;

    void Awake()
    {
        if (Current > Max || Current <= 0)
        {
            Current = Max;
        }
    }

    void Start()
    {
        _listenerList = ListenerList;
    }

    public void Register(ListenerBase listener)
    {
        _listenerList.Add(listener);
    }

    public void Deregister(ListenerBase listener)
    {
        _listenerList.Remove(listener);
    }

    public void TakeDamage(float amount)
    {
        Current = Mathf.Max(0, Current - amount);

        if (Current <= 0)
        {
            foreach(ListenerBase listener in _listenerList)
            {
                listener.Fire();
            }
        }
    }

    public void Heal(float amount)
    {
        Current = Mathf.Min(Max, Current + amount);
    }
}

