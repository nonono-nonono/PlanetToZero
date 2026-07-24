using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickContext: EventContext
{
    public Vector2 MousePos;
    public ClickContext(Vector2 worldPos) => MousePos = worldPos;
}

public class Click : MonoBehaviour, IListenable
{
    [field: SerializeField] private List<ListenerBase> _listenerList;

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

    void OnClick(InputValue value)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        foreach (ListenerBase listener in _listenerList)
        {
            // Implicit type conversion from Vector3 --> Vector2
            listener.Fire(new ClickContext(worldPos));
        }
    }
}
