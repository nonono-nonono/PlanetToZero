using System;
using System.Collections.Generic;

// Helper for IDynamicListenables. Provides a registry for listeners.
public class ListenerRegistry<T> where T : Enum
{
    private Dictionary<T, List<ListenerBase>> _listeners = new();

    // Registers listeners to an event.
    public void Register(T eventType, ListenerBase listener)
    {
        // Checks if list of listeners for event given exists. If not create a list for it.
        if (!_listeners.TryGetValue(eventType, out List<ListenerBase> list))
        {
            list = new();
            _listeners[eventType] = list;
        }

        list.Add(listener);
    }

    // Deregisters listeners from an event.
    public void Deregister(T eventType, ListenerBase listener)
    {
        // Checks if list of listeners for event given exists.
        if (!_listeners.TryGetValue(eventType, out List<ListenerBase> list))
        {
           list.Remove(listener);
        }  
    }

    // Returns an array of listeners given an event.
    public ListenerBase[] FetchListenersByType(T eventType)
    {   
        // Ternary operator: bool ? trueResult : falseResult
        return _listeners.TryGetValue(eventType, out List<ListenerBase> list) ? list.ToArray() : Array.Empty<ListenerBase>();
    }
}
