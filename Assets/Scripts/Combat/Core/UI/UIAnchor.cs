using UnityEngine;
using UnityEngine.Rendering.UI;

public class UIAnchor : MonoBehaviour
{
    public RectTransform Panel;

    void OnEnable()
    {
        UIAnchorManager.Instance.Register(this);
    }

    void OnDisable()
    {
        UIAnchorManager.Instance.Deregister(this);
    }
}
