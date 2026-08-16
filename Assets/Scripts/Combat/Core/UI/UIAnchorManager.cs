using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Manages all UI Anchors and ensures panel for each ui anchor stays anchored even as ui anchor position changes.
public class UIAnchorManager : MonoBehaviour
{
    public static UIAnchorManager Instance;
    [SerializeField] private Canvas _enemyCanvas;
    private Dictionary<UIAnchor, RectTransform> _uiRegistry = new();
    private Queue<(UIAnchor uiAnchor, RectTransform rectTranform, bool addingToCanvas)> _parentingQueue = new();

    // Singleton pattern to ensure only 1 ui anchor manager exists.
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Queue up rect transforms to be either reparented or destroyed.
    // Not done synchronously because Unity's delayed destruction system can still be happening and I cannot exactly know the state of the ui anchor/rect transform.
    // Waits until unity resolves state of ui anchor/rect transform before deciding.
    void Update()
    {
        // While loop that runs if there are things in queue
        while (_parentingQueue.Count > 0)
        {   
            // Removes most recent element from queue
            var (uiAnchor, rectTranform, addingToCanvas) = _parentingQueue.Dequeue();

            // Sees if rect transform wants to be added or removed from the canvas.
            // If true, adds to canvas, otherwise, either destroy or reparent rect transform to ui anchor based on destruction state of ui anchor/rect transform. 
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
                    if (rectTranform != null) Destroy(rectTranform.gameObject);
                }
            }
        }
    }

    // Move given panel to ui anchor position on LateUpdate() to ensure ui anchor position is changed before panel moves to that area.
    void LateUpdate()
    {
        foreach ((UIAnchor uiAnchor, RectTransform actualUI) in _uiRegistry)
        {
            actualUI.position = uiAnchor.transform.position;
        }
    }

    // Registers a ui anchor to the manager at runtime.
    public void Register(UIAnchor uiAnchor)
    {
        // If ui anchor is already registered, ignore.
        if (_uiRegistry.TryGetValue(uiAnchor, out var _))
        {
            Debug.LogWarning($"Tried to register {uiAnchor} when it is already registered!");
            return;
        }

        // Gets rect transform and inserts it into a dictionary where ui anchor is the key and rect transform is the value. Queue the panel to be parented into the world canvas.
        RectTransform rectTransform = uiAnchor.Panel.GetComponent<RectTransform>();

        _uiRegistry[uiAnchor] = rectTransform;
        _parentingQueue.Enqueue((uiAnchor, rectTransform, true));
    }


    // Deregisters a ui anchor to the manager at runtime.
    public void Deregister(UIAnchor uiAnchor)
    {
        // If ui anchor isn't registered, ignore.
        if (!_uiRegistry.TryGetValue(uiAnchor, out RectTransform rectTransform))
        {
            Debug.LogWarning($"Tried to Deregister {uiAnchor} when it is not registered!");
            return;
        }
        
        // Removes the key value pair for given ui anchor in the ui registry. Queue the panel to be parented back to the ui anchor.
        _uiRegistry.Remove(uiAnchor);
        _parentingQueue.Enqueue((uiAnchor, rectTransform, false));
    }
}
