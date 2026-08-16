using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Context given to all listeners. Implements IPosition Context.
public class ClickContext: EventContext, IPositionContext
{
    public Vector2 TargetPosition {get;}
    public ClickContext(Vector2 worldPos) => TargetPosition = worldPos;
}

// Click listenable. Fires all its listeners with click context when the player clicks.
public class Click : MonoBehaviour, IListenable
{
    [SerializeField] private List<ListenerBase> _listenerList;

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

    // Click is an action on the Player Action map. It fires everytime the player clicks.
    // OnClick plugs into that action through "On[ActionName]" and runs everytime Click is fired.
    void OnClick(InputValue value)
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        // Gets world position of where player clicked from screenPos (where mouse currently is in pixel position)
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        // Fires each listener attached to it with new click context containing mouse world position.
        foreach (ListenerBase listener in _listenerList)
        {
            // Implicit type conversion from Vector3 --> Vector2
            listener.Fire(new ClickContext(worldPos));
        }
    }
}
