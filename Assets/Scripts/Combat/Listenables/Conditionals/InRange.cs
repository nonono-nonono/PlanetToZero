using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class RangeContext : EventContext, IConditionalContext, IPositionContext
{
    public bool ConditionMet {get;}
    public Vector2 TargetPosition {get;}

    public RangeContext(bool conditionMet, Vector2 targetPos)
    {
        TargetPosition = targetPos;
        ConditionMet = conditionMet;
    }
}

public class InRange : MonoBehaviour, IListenable, IContextPullable
{
    [SerializeField] private Transform _originTransform;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float range;
    [field: SerializeField] private List<ListenerBase> _listenerList;
    private bool _inRange;

    void Update()
    {
        bool isRangeChanged = CheckRange();

        if (isRangeChanged)
        {
            foreach (ListenerBase listener in _listenerList)
            {
                listener.Fire(new RangeContext(_inRange, _targetTransform.position));
            }
        }
    }

    private bool CheckRange()
    {
        float distance = (transform.position - _targetTransform.position).magnitude;

        if (distance <= range && !_inRange)
        {
            _inRange = true;
            return true;
        }

        if (distance > range && _inRange)
        {
            _inRange = false;
            return true;
        }

        return false;
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

    public EventContext GrabContext()
    {
        CheckRange();

        return new RangeContext(_inRange, _targetTransform.position);
    }
}
