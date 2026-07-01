using System.Collections.Generic;
using UnityEngine;

public abstract class ListenerBase : MonoBehaviour
{
    [SerializeField] protected List<ReactionBase> Reactions;
    public abstract void Fire();
}
