using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float _speed;
    private Vector2 _direction;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (!_rb)
        {
            Debug.Log($"{gameObject} lacks a RigidBody2D. Failed to add player movement!");
        }
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
