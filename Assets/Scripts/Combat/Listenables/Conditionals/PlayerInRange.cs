using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Context given when player transform enters/exits the range of this component's transform. Implements IConditionalContext and IPositionContext.
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

// Check if player transform is within a specific range.
public class PlayerInRange : MonoBehaviour, IListenable, IContextPullable
{
    [SerializeField] private Transform _originTransform;
    [SerializeField] private float range;
    [SerializeField] private List<ListenerBase> _listenerList;
    private bool _inRange;
    private Transform _targetTransform;

    // Get Player object from game manager on start.
    // Player game object could not have registered itself yet so if it doesn't exist, defer until it has been registered.
    void Start()
    {
        if (GameManager.PlayerObject != null)
        {
            _targetTransform = GameManager.PlayerObject.transform;
            return;
        }
        StartCoroutine(GetPlayerReference());
    }

    void Update()
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;

        // Ignores if targetted transform (player) doesn't exist.
        if (_targetTransform == null) return;

        // Check if whether the player is in range has changed.
        bool isRangeChanged = CheckRangeState();

        // Fire all listeners with RangeContext on whether the player is in range and the player's current position.
        if (isRangeChanged)
        {
            foreach (ListenerBase listener in _listenerList)
            {
                listener.Fire(new RangeContext(_inRange, _targetTransform.position));
            }
        }
    }
    
    private IEnumerator GetPlayerReference()
    {
        while (GameManager.PlayerObject == null) yield return null;
        _targetTransform = GameManager.PlayerObject.transform;
    }

    // Check if whether the player is in range has changed.
    private bool CheckRangeState()
    {
        // Returns false if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return false;

        // Returns false if targetted transform (player) doesn't exist.
        if (_targetTransform == null) return false;

        // Distance between component transform and player transform
        float distance = (transform.position - _targetTransform.position).magnitude;

        // See if current player state of whether it is in range is different.
        // If it is different, set _inRange to whether the player is in range or not and return true.
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

        // No change to current player state of whether it is in range, just return false.
        return false;
    }

    // Part of IListenable inteface implementation.
    public void Register(ListenerBase listener)
    {
        _listenerList.Add(listener);
    }

    // Part of IListenable inteface implementation.
    public void Deregister(ListenerBase listener)
    {
        _listenerList.Remove(listener);
    }

    // Part of IListenable inteface implementation.
    public ListenerBase[] FetchListeners()
    {
        return _listenerList.ToArray();
    }

    // Part of IContextPullable implementation.
    public EventContext GrabContext()
    {
        // Returns null if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return null;
        
        // Returns null if targetted transform (player) does not exist.
        if (_targetTransform == null) return null;
        
        // Check if whether the player is in range has changed.
        CheckRangeState();

        // Return new RangeContext with updated _inRange info
        return new RangeContext(_inRange, _targetTransform.position);
    }
}
