using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TreeBehaviour1 : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private MovementState _currentState;
    private Vector3 _startingPos;
    [SerializeField] private LayerMask _playerLayer;
    private Transform _playerTransform;
    [SerializeField] private float _searchRange, _maxDistance, _attackDistance;
    [SerializeField] private float _speed, _speedReductor;
    private float _maxSpeed;
    private bool _lookingRight;

    [Header("Attack")]
    [SerializeField] private GameObject _projectilePrefab; // Prefab del proyectil
    [SerializeField] private Transform _firePoint; // Punto desde donde se dispara el proyectil
    [SerializeField] private float _reloadTime = 2f; // Tiempo de recarga entre disparos
    private float _lastAttackTime; // Momento del �ltimo ataque

    enum EnemyType
    {
        Mushroom,
        WanderingSoul,
        MagicCloud,
    }

    public enum MovementState
    {
        Waiting,
        Following,
        Returning,
        Attacking,
    }

    private void Start()
    {
        _startingPos = transform.position;
        _maxSpeed = _speed;
    }

    private void Update()
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
            case MovementState.Attacking:
                AttackingState();
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
        else if (Vector2.Distance(transform.position, _playerTransform.position) < _attackDistance)
        {
            _currentState = MovementState.Attacking;
        }
    }

    private void ReturnningState()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            _startingPos, _speed * Time.deltaTime);

        FlipToTarget(_startingPos);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRange, _playerLayer);

        if (colliders.Length > 0)
        {
            _playerTransform = colliders[0].transform;
            _currentState = MovementState.Following;
        }

        if (Vector2.Distance(transform.position, _startingPos) < 0.1)
        {
            _currentState = MovementState.Waiting;
        }
    }

    private void AttackingState()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            _playerTransform.position, _speed / _speedReductor * Time.deltaTime);

        if (_playerTransform == null)
        {
            _currentState = MovementState.Returning;
            return;
        }

        FlipToTarget(_playerTransform.position);

        Attack();

        Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRange, _playerLayer);

        if (colliders.Length > 0 && Vector2.Distance(transform.position, _playerTransform.position) > _attackDistance)
        {
            _speed = _maxSpeed;
            _playerTransform = colliders[0].transform;
            _currentState = MovementState.Following;
        }
        else if (Vector2.Distance(transform.position, _startingPos) > _maxDistance ||
            Vector2.Distance(transform.position, _playerTransform.position) > _maxDistance)
        {
            _speed = _maxSpeed;
            _currentState = MovementState.Returning;
            _playerTransform = null;
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
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void Attack()
    {
        switch (_enemyType)
        {
            case EnemyType.Mushroom:
                AttackMushroom();
                break;
            case EnemyType.WanderingSoul:
                AttackWanderingSoul();
                break;
            case EnemyType.MagicCloud:
                AttackMagicCloud();
                break;
        }
    }

    private void AttackMushroom()
    {

    }

    private void AttackWanderingSoul()
    {
        if (Time.time >= _lastAttackTime + _reloadTime)
        {
            ShootProjectile();
            _lastAttackTime = Time.time;
        }
    }

    private void AttackMagicCloud()
    {

    }

    private void ShootProjectile()
    {
        if (_playerTransform == null)
        {
            return;

        }

        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position,
            Quaternion.identity); // Instanciar el disparo desde la posicion de disparo
        Vector3 direction = (_playerTransform.position - 
            _firePoint.position).normalized; // Direccion al jug
        projectile.transform.rotation = Quaternion.LookRotation(direction); // Rota el desparo

        // Aplicar velocidad al proyectil
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * 10f; // Ajusta la velocidad según sea necesario
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _searchRange);
        Gizmos.DrawWireSphere(_startingPos, _maxDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }
}
