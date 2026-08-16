using System;

// Regular listenables only have 1 event. (1 hook to for listners to hook on)
public interface IListenable
{
    ListenerBase[] FetchListeners();
    void Register(ListenerBase listener);
    void Deregister(ListenerBase listener);
}

// Dynamic listenables can have many events (many hooks for listeners to hook on)
public interface IDynamicListenable<T> where T : Enum
{
    ListenerBase[] FetchListenersByType(T eventType);
    void Register(T eventType, ListenerBase listener);
    void Deregister(T eventType, ListenerBase listener);
}

public interface IContextPullable
{
    EventContext GrabContext();
}