using System;
using UnityEngine;

public class Death : ReactionBase
{
    [SerializeField] private GameObject _target;

    public override void Execute()
    {
        Destroy(_target);
    }
}
