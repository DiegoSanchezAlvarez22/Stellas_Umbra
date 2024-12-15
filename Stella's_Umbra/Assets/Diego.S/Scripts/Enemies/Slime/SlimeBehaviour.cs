using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class SlimeBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private bool _rightMovement;
    [SerializeField] private float _speed;
    [SerializeField] private float _actualSpeed;
    [SerializeField] private float _distanceLineDown;
    [SerializeField] private float _distanceLineRight;
    [SerializeField] private Transform _floorFinder;
    private Rigidbody _rb;

    [Header("Attack")]
    [SerializeField] private int _contactDamage;
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _rechargeAttackTime;
    private float _distanceToPlayer;
    [SerializeField] private float _distanceMoveToPlayer;
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _tiempoAtacando;
    private Animator _anim;
    private Transform _player;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        _distanceToPlayer = Vector2.Distance(transform.position, _player.position);
    }

    void FixedUpdate()
    {
        if (!Physics.Raycast(_floorFinder.position, Vector3.down, out RaycastHit _floor, _distanceLineDown))
        {
            Girar();
        }
        if (Physics.Raycast(_floorFinder.position, Vector3.right, out RaycastHit _wallRight, _distanceLineRight)
            || Physics.Raycast(_floorFinder.position, Vector3.left, out RaycastHit _wallLeft, _distanceLineRight))
        {
            Girar();
        }

        Move();
    }

    private void Move()
    {
        if (_distanceToPlayer >= _distanceMoveToPlayer)
        {
            _rb.linearVelocity = new Vector3(_actualSpeed, _rb.linearVelocity.y, _rb.linearVelocity.z);
        }

        if (_distanceToPlayer < _distanceMoveToPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.position,
                _actualSpeed * Time.deltaTime);
        }

        if (_distanceToPlayer < _attackDistance)
        {
            _actualSpeed = 0;
            _anim.SetTrigger("isAttack");
            Invoke("Timer", _tiempoAtacando);
        }
    }

    void Timer()
    {
        _actualSpeed = _speed;
    }

    private void Girar()
    {
        _rightMovement = !_rightMovement;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        _actualSpeed *= -1;
        _speed *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_floorFinder.transform.position,
            _floorFinder.transform.position + Vector3.down * _distanceLineDown);
        Gizmos.DrawLine(_floorFinder.transform.position,
            _floorFinder.transform.position + Vector3.left * _distanceLineRight);
        Gizmos.DrawLine(_floorFinder.transform.position,
            _floorFinder.transform.position + Vector3.right * _distanceLineRight);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _distanceToPlayer);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _distanceMoveToPlayer);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }
}
