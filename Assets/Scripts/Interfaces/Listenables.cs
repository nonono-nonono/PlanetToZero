using System;
using System.Collections.Generic;

public interface IListenable
{
    ListenerBase[] FetchListeners();
    void Register(ListenerBase listener);
    void Deregister(ListenerBase listener);
}

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