using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIAnchorManager : MonoBehaviour
{
    public static UIAnchorManager Instance;
    [SerializeField] private Canvas _enemyCanvas;
    private Dictionary<UIAnchor, RectTransform> _uiRegistry = new();

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
        foreach ((UIAnchor uiAnchor, RectTransform actualUI) in _uiRegistry)
        {
            actualUI.position = uiAnchor.transform.position;
        }
    }

    public void Register(UIAnchor uiAnchor)
    {
        if (_uiRegistry.TryGetValue(uiAnchor, out var _))
        {
            Debug.LogWarning($"Tried to register {uiAnchor} when it is already registered!");
            return;
        }

        RectTransform rectTransform = uiAnchor.Panel.GetComponent<RectTransform>();
        if (rectTransform == null) 
        {

            Debug.LogError($"Panel of {uiAnchor} has no rectTransform!");
            return;

        }

        _uiRegistry[uiAnchor] = rectTransform;
        uiAnchor.Panel.transform.SetParent(_enemyCanvas.transform);
    }

    public void Deregister(UIAnchor uiAnchor)
    {
        if (!_uiRegistry.TryGetValue(uiAnchor, out RectTransform rectTransform))
        {
            Debug.LogWarning($"Tried to Deregister {uiAnchor} when it is not registered!");
            return;
        }
        
        _uiRegistry.Remove(uiAnchor);

        if (uiAnchor != null && rectTransform != null)
        {
            rectTransform.transform.SetParent(uiAnchor.transform);
        }
    }
}
