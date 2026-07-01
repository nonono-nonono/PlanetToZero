public interface IListenable
{
    void Register(ListenerBase listener);
    void Deregister(ListenerBase listener);
}
