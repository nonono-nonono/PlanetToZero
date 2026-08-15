using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _follow;

    void LateUpdate()
    {
        if (_follow == null) return;
        transform.position = new Vector3(_follow.position.x, _follow.position.y, -10);
    }
}
