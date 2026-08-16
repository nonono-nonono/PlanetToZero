using UnityEngine;
using UnityEngine.InputSystem;

// Adds WASD control to player
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    // Bounds is on a different layer never triggered by physics, only using it as data source and easy change of map bounds.
    [SerializeField] private Collider2D _bounds;
    [SerializeField] private float _speed;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private Collider2D _cd;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cd = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _direction * _speed;

        if (_bounds != null)
        {
            Bounds mapBounds = _bounds.bounds;
            Bounds playerBounds = _cd.bounds;

            // Half extents from center
            float plrHalfWidth = playerBounds.extents.x;
            float plrHalfHeight = playerBounds.extents.y;

            float minX = mapBounds.min.x + plrHalfWidth;
            float maxX = mapBounds.max.x - plrHalfWidth;
            float minY = mapBounds.min.y + plrHalfHeight;
            float maxY = mapBounds.max.y - plrHalfHeight;

            Vector2 currentPos = _rb.position;

            // Clamping x and y coordinates in a specific range. (Clamp just means keep within a range)
            float clampedX = Mathf.Clamp(currentPos.x, minX, maxX);
            float clampedY = Mathf.Clamp(currentPos.y, minY, maxY);

            // Sets rigidbody position to clamped position
            _rb.position = new Vector3(clampedX, clampedY, 0);
        }
    }

    // Move is an action on the Player Action map. It fires everytime the player presses WASD.
    // OnMove plugs into that action through "On[ActionName]" and runs everytime WASD is pressed.
    // Each press returns a normalized direction value (even for W + A and W + D)
    void OnMove(InputValue value)
    {
       _direction = value.Get<Vector2>();
    }
}
