using UnityEngine;

// Stores the origin prefab that the bullet instance was created from.
public class PooledBullet : MonoBehaviour
{
    [HideInInspector] public GameObject OriginPrefab;
}
