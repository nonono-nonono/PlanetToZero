using System.Collections.Generic;
using UnityEngine;

public class onLifecycleEnd : ListenerBase
{
    private ILifecycle _lifecycleComp;

    void Start()
    {
        _lifecycleComp = GetComponent<ILifecycle>();

        if (_lifecycleComp == null)
        {
            Debug.Log($"Could not add onLifecycleEnd to {gameObject}! Missing a Lifecycle component!");
        }
        else
        {
            _lifecycleComp.OnLifecycleEnd += Fire;
        }
    }

    void Fire()
    {
        foreach (IReaction reaction in Reactions)
        {
            reaction.Execute();
        }
    }
}
