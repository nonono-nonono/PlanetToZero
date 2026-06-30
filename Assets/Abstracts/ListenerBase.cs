using System.Collections.Generic;
using UnityEngine;

public abstract class ListenerBase : MonoBehaviour
{
    [field: SerializeField] public List<IReaction> Reactions {get; private set;} = new();
}
