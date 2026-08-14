using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeContext : EventContext, IConditionalContext
{
    public bool ConditionMet {get;}

    public RangeContext(bool conditionMet)
    {
        ConditionMet = conditionMet;
    }
}

public class InRange : MonoBehaviour, IListenable
{
    [SerializeField] private Transform _originTransform;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float range;
    [field: SerializeField] private List<ListenerBase> _listenerList;
    private bool _inRange;

    void Update()
    {
        float distance = (transform.position - _targetTransform.position).magnitude;

        if (distance <= range && !_inRange)
        {
            _inRange = true;
            foreach (ListenerBase listener in _listenerList)
            {
                listener.Fire(new RangeContext(true));
            }
            return;
        }

        if (distance > range && _inRange)
        {
            _inRange = false;
            foreach (ListenerBase listener in _listenerList)
            {
                listener.Fire(new RangeContext(false));
            }
            return;
        }
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
