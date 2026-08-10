using UnityEngine;
using UnityEngine.Rendering.UI;

[ExecuteAlways]
public class UIAnchor : MonoBehaviour
{
    public RectTransform PanelParent;

    void OnEnable()
    {
        UIAnchorManager.Instance.Register(this);
    }

    void OnDisable()
    {
        UIAnchorManager.Instance.Deregister(this);
    }
}
