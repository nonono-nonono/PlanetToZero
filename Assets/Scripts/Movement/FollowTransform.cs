using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowTransform : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _follow;
    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_follow == null) return;
        
        Vector2 currentPos = _rb.position;
        Vector2 targetPos = _follow.position;

        Vector2 toPos = targetPos - currentPos;
        float distanceCurrentFrame = _speed * Time.fixedDeltaTime;

        if (toPos.magnitude < distanceCurrentFrame)
        {
            _rb.MovePosition(targetPos);
        }
        else
        {
           _rb.MovePosition(currentPos + distanceCurrentFrame  * toPos.normalized);  
        }
    }
}
