using System;

public interface IDynamicListenable<T> where T : Enum
{
    void Register(T eventType, ListenerBase listener);
    void Deregister(T eventType, ListenerBase listener);
}
