using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector2 _direction;
    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _direction * _speed;
    }

    void OnMove(InputValue value)
    {
       _direction = value.Get<Vector2>();
    }
}
