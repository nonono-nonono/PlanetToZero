using System;
using UnityEngine;

public class Death : MonoBehaviour, IReaction
{
    [SerializeField] private GameObject _target;

    public void Execute()
    {
        Destroy(_target);
    }
}
