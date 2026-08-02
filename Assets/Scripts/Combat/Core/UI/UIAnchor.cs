using UnityEngine;
using UnityEngine.Rendering.UI;

public class UIAnchor : MonoBehaviour
{
    public GameObject UIPrefab;

    void OnEnable()
    {
        UIAnchorManager.Instance.Register(this);
    }

    void OnDisable()
    {
        UIAnchorManager.Instance.Deregister(this);
    }
}
