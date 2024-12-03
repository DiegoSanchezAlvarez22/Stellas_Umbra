using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    [SerializeField] private float _searchRange;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private float _speed;
    [SerializeField] private float _maxDistance;
    [SerializeField] private Vector3 _startingPos;

    [SerializeField] private bool _lookingRight;

    [SerializeField] private MovementState _currentState;

    public enum MovementState
    {
        Waiting,
        Following,
        Returning,
    }

    private void Start()
    {
        _startingPos = transform.position;
    }

    void Update()
    {
        switch (_currentState)
        {
            case MovementState.Waiting:
                WaitingState();
                break;
            case MovementState.Following:
                FollowingState();
                break;
            case MovementState.Returning:
                ReturnningState();
                break;
        }
    }

    private void WaitingState()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRange, _playerLayer);

        if (colliders.Length > 0)
        {
            _playerTransform = colliders[0].transform;
            _currentState = MovementState.Following;
        }
        else
        {
            _playerTransform = null;
        }
    }

    private void FollowingState()
    {
        if (_playerTransform == null)
        {
            _currentState = MovementState.Returning;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, 
            _playerTransform.position, _speed * Time.deltaTime);

        FlipToTarget(_playerTransform.position);

        if (Vector2.Distance(transform.position, _startingPos) > _maxDistance ||
            Vector2.Distance(transform.position, _playerTransform.position) > _maxDistance)
        {
            _currentState = MovementState.Returning;
            _playerTransform = null;
        }
    }

    private void ReturnningState()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            _startingPos, _speed * Time.deltaTime);

        FlipToTarget(_startingPos);

        if (Vector2.Distance(transform.position, _startingPos) < 0.1)
        {
            _currentState = MovementState.Waiting;
        }
    }

    private void FlipToTarget(Vector3 _target)
    {
        if (_target.x > transform.position.x && !_lookingRight)
        {
            Flip();
        }
        else if (_target.x < transform.position.x && _lookingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        _lookingRight = !_lookingRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y +180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _searchRange);
        Gizmos.DrawWireSphere(_startingPos, _maxDistance);
    }
}
