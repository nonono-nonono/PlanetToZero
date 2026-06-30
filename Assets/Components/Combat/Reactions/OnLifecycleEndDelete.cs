using UnityEngine;

public class onLifecycleEndDelete : MonoBehaviour
{
    private ILifecycle _lifecycleComp;

    void Start()
    {
        _lifecycleComp = GetComponent<ILifecycle>();

        if (_lifecycleComp == null)
        {
            Debug.Log($"Could not add OnDeathDelete to {gameObject}! Missing a health component!");
        }
        else
        {
            _lifecycleComp.OnLifecycleEnd += HandleDeath;
        }
    }

    void HandleDeath()
    {
        Destroy(gameObject);
    }
}
