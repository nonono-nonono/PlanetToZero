using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class UIAnchor : MonoBehaviour
{
    public RectTransform Panel;

    void OnEnable()
    {
        if (UIAnchorManager.Instance == null)
        {
            StartCoroutine(RegisterRoutine());
            return;
        }

        UIAnchorManager.Instance.Register(this);
    }

    void OnDisable()
    {
        UIAnchorManager.Instance.Deregister(this);
    }

    IEnumerator RegisterRoutine()
    {
        while (UIAnchorManager.Instance == null) yield return null;

        UIAnchorManager.Instance.Register(this);
    }
}
