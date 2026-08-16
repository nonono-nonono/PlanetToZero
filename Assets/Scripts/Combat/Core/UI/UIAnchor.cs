using System.Collections;
using UnityEngine;

// Anchors a panel (containing further more ui elements) to a position. (Position can change overtime and it still anchors)
public class UIAnchor : MonoBehaviour
{
    public RectTransform Panel;

    // Registers ui anchor when enabled.
    // UIAnchorManager not guaranteed to exist on first OnEnable() (Execution order: Awake + OnEnable for all game objects then Start()) so defer until it exists.
    void OnEnable()
    {
        if (UIAnchorManager.Instance != null)
        {
            UIAnchorManager.Instance.Register(this); 
            return;
        }

        StartCoroutine(DeferredRegister());
    }

    // Deregisters ui anchor when disabled.
    void OnDisable()
    {
        UIAnchorManager.Instance.Deregister(this);
    }

    private IEnumerator DeferredRegister()
    {
        while (UIAnchorManager.Instance == null) yield return null;
        UIAnchorManager.Instance.Register(this); 
    }
}
