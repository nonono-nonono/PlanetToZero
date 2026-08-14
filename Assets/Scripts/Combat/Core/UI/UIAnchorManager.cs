using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIAnchorManager : MonoBehaviour
{
    public static UIAnchorManager Instance;
    [SerializeField] private Canvas _enemyCanvas;
    private Dictionary<UIAnchor, RectTransform> _uiRegistry = new();
    private Queue<(UIAnchor uiAnchor, RectTransform rectTranform, bool addingToCanvas)> _parentingQueue = new();

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
        while (_parentingQueue.Count > 0)
        {
            var (uiAnchor, rectTranform, addingToCanvas) = _parentingQueue.Dequeue();

            if (addingToCanvas)
            {
                rectTranform.transform.SetParent(_enemyCanvas.transform);
            }
            else
            {
                if (uiAnchor != null && rectTranform != null)
                {
                    rectTranform.transform.SetParent(uiAnchor.transform);
                } 
                else
                {
                    Destroy(rectTranform.gameObject);
                }
            }
        }
    }

    void LateUpdate()
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
        _parentingQueue.Enqueue((uiAnchor, rectTransform, true));
    }

    public void Deregister(UIAnchor uiAnchor)
    {
        if (!_uiRegistry.TryGetValue(uiAnchor, out RectTransform rectTransform))
        {
            Debug.LogWarning($"Tried to Deregister {uiAnchor} when it is not registered!");
            return;
        }
        
        _uiRegistry.Remove(uiAnchor);
        _parentingQueue.Enqueue((uiAnchor, rectTransform, false));
    }
}
