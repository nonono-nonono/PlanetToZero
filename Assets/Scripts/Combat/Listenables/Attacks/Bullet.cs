using System.Collections.Generic;
using UnityEngine;

public class BulletContext : EventContext
{
    public GameObject hit;
}

// Simple Bullet, Targets a specific team only, deletes itself and returns the first game object with an attack manager of the target team on hit.
public class Bullet : MonoBehaviour, IListenable
{
    [field: SerializeField] public Team TargetTeam {get; private set;}
    [field: SerializeField] private List<ListenerBase> _listenerList;
    private bool canHit; 

    public void Activate()
    {
        canHit = true;
    }

    public void Register(ListenerBase listener)
    {
        _listenerList.Add(listener);
    }

    public void Deregister(ListenerBase listener)
    {
        _listenerList.Remove(listener);
    }

    public ListenerBase[] FetchListeners()
    {
        return _listenerList.ToArray();
    }
}
