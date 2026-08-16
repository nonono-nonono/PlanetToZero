using System.Collections;
using UnityEngine;

// Makes camera follow the player, stays within bounds of a map.
[RequireComponent(typeof(Camera))]
public class CameraFollowPlayer : MonoBehaviour
{
    // Bounds is on a different layer never triggered by physics, only using it as data source and easy change of map bounds.
    [SerializeField] private Collider2D _bounds;
    private Transform _follow;
    private Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();
    }
    
    void Start()
    {
        // If PlayerObject does not exist yet, set _follow to its transform, otherwise defer setting _follow until PlayerObject exists.
        if (GameManager.PlayerObject != null)
        {
           _follow = GameManager.PlayerObject.transform;
           return;
        }

        StartCoroutine(GetPlayerReference());
    }

    // Sets camera position to player object within bounds. LateUpate() to ensure player object has already been updated before moving camera.
    void LateUpdate()
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;

        if (_follow == null) return;

        Vector3 targetPos = new(_follow.position.x, _follow.position.y, -10);

        // Keep camera within bounds if it exists.
        if (_bounds != null)
        {
            // aspect = width / height. Half extents of the camera.
            float camHalfHeight = _cam.orthographicSize;
            float camHalfWidth = camHalfHeight * _cam.aspect;

            Bounds bounds = _bounds.bounds;

            float minX = bounds.min.x + camHalfWidth;
            float maxX = bounds.max.x - camHalfWidth;
            float minY = bounds.min.y + camHalfHeight;
            float maxY = bounds.max.y - camHalfHeight;

            // Clamping x and y coordinates in a specific range. (Clamp just means keep within a range)
            float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
            float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

            targetPos = new(clampedX, clampedY, -10);
        }

        transform.position = targetPos;
    }

    private IEnumerator GetPlayerReference()
    {
        while (GameManager.PlayerObject == null) yield return null;
        _follow = GameManager.PlayerObject.transform;
    }
}
