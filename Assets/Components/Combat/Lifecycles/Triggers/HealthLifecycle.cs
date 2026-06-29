using UnityEngine;
using UnityEngine.Events;

public class HealthLifecycle : MonoBehaviour, ILifecycle
{
    [field: SerializeField] public float Max {get; private set;}
    [field: SerializeField] public float Current {get; private set;}
    public event UnityAction OnLifecycleEnd;

    void Awake()
    {
        if (Current > Max || Current <= 0)
        {
            Current = Max;
        }
    }

    public void TakeDamage(float amount)
    {
        Current = Mathf.Max(0, Current - amount);

        if (Current <= 0)
        {
            OnLifecycleEnd?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        Current = Mathf.Min(Max, Current + amount);
    }
}

