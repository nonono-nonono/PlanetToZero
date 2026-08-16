using System.Collections;
using UnityEngine;

// Moves towards player object at a given speed.
[RequireComponent(typeof(Rigidbody2D))]
public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Transform _follow;
    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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

    void FixedUpdate()
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;

        if (_follow == null) return;
        
        Vector2 currentPos = _rb.position;
        Vector2 targetPos = _follow.position;

        Vector2 toPos = targetPos - currentPos;
        float distanceCurrentFrame = _speed * Time.fixedDeltaTime;

        // If distance between player object and this rigidbody is less than the distance it would cover this frame, just move to target position.
        // Otherwise move by distanceCurrentFrame.
        // This condition prevents constant jittering.
        if (toPos.magnitude < distanceCurrentFrame)
        {
            _rb.MovePosition(targetPos);
        }
        else
        {
           _rb.MovePosition(currentPos + distanceCurrentFrame * toPos.normalized);  
        }
    }

    private IEnumerator GetPlayerReference()
    {
        if (GameManager.PlayerObject == null) yield return null;
        _follow = GameManager.PlayerObject.transform;
    }
}
