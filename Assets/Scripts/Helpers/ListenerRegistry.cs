using System;
using System.Collections.Generic;

public class ListenerRegistry<T> where T : Enum
{
    private readonly Dictionary<T, List<ListenerBase>> _listeners = new();

    public void Register(T eventType, ListenerBase listener)
    {
        if (!_listeners.TryGetValue(eventType, out List<ListenerBase> list))
        {
            list = new List<ListenerBase>();
            _listeners[eventType] = list;
        }

        list.Add(listener);
    }

    public void Deregister(T eventType, ListenerBase listener)
    {
        if (!_listeners.TryGetValue(eventType, out List<ListenerBase> list))
        {
           list.Remove(listener);
        }  
    }

    public ListenerBase[] FetchListenersByType(T eventType)
    {
        return _listeners.TryGetValue(eventType, out List<ListenerBase> list) ? list.ToArray() : Array.Empty<ListenerBase>();
    }
}
