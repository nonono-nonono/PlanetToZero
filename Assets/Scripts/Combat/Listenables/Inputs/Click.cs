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
    [field: SerializeField] public List<ListenerBase> ListenerList {get; private set;}

    public void Register(ListenerBase listener)
    {
        ListenerList.Add(listener);
    }

    public void Deregister(ListenerBase listener)
    {
        ListenerList.Remove(listener);
    }

    void OnClick(InputValue value)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        foreach (ListenerBase listener in ListenerList)
        {
            // Implicit type conversion from Vector3 --> Vector2
            listener.Fire(new ClickContext(worldPos));
        }
    }
}
