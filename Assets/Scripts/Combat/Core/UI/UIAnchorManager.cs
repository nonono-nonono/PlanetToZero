using System.Collections.Generic;
using UnityEngine;

public class UIAnchorManager : MonoBehaviour
{
    public static UIAnchorManager Instance;
    [SerializeField] private Canvas _enemyCanvas;
    private Dictionary<UIAnchor, GameObject> _uiRegistry;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        foreach ((UIAnchor uiAnchor, GameObject ui) in _uiRegistry)
        {
            ui.transform.position = uiAnchor.transform.position;
        }
    }

    public void Register(UIAnchor uiAnchor)
    {
        if (_uiRegistry[uiAnchor] == null)
        {
            _uiRegistry[uiAnchor] = Instantiate(uiAnchor.gameObject, _enemyCanvas.transform);
        }
        else
        {
            Debug.LogWarning($"Tried to register {uiAnchor} when it is already registered!");
        }
    }

    public void Deregister(UIAnchor uiAnchor)
    {
        if (_uiRegistry[uiAnchor] != null)
        {
            Destroy( _uiRegistry[uiAnchor]);
            _uiRegistry[uiAnchor] = null;
        }
        else
        {
            Debug.LogWarning($"Tried to Deregister {uiAnchor} when it is not registered!");
        }
    }
}
