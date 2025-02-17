using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyLifes))]
public class SlimeBehaviour : MonoBehaviour
{
    #region Variables
    
    [Header("Movement")] // ---------- CONFIGURACI�N DEL MOVIMIENTO ----------
    [SerializeField] private bool _rightMovement; // Indica si el enemigo se mueve hacia la derecha
    [SerializeField] private float _speed; // Velocidad base del enemigo
    [HideInInspector] private float _actualSpeed; // Velocidad actual (puede modificarse en ciertas situaciones)
    [HideInInspector] private float _distanceLineDown = 1f; // Distancia del raycast para detectar el suelo
    [HideInInspector] private float _distanceLineRight = 0.1f; // Distancia del raycast para detectar paredes
    [SerializeField] private Transform _floorFinder; // Objeto vac�o que se usa como origen del raycast hacia el suelo
    [SerializeField] private LayerMask _floorLayer; // Capa que representa el suelo
    [SerializeField] private LayerMask _wallLayer; // Capa que representa las paredes
    private Rigidbody _rb; // Referencia al Rigidbody del enemigo

    [Header("Attack")] // ---------- CONFIGURACI�N DEL ATAQUE ----------
    [SerializeField] private int _contactDamage; // Da�o que hace el enemigo al tocar al jugador
    [SerializeField] private int _attackDamage; // Da�o que hace el enemigo con su ataque
    [SerializeField] private float _rechargeAttackTime; // Tiempo de recarga entre ataques
    [SerializeField] private float _distanceMoveToPlayer; // Distancia a la que el enemigo comienza a moverse hacia el jugador
    [SerializeField] private float _attackDistance; // Distancia a la que el enemigo comienza a atacar
    [SerializeField] private float _tiempoAtacando; // Tiempo que dura la animaci�n de ataque
    private float _distanceToPlayer; // Distancia actual entre el enemigo y el jugador
    private Animator _anim; // Referencia al Animator para controlar animaciones
    private Transform _player; // Referencia al transform del jugador

    #endregion

    void Start()
    {
        // Obtener componentes y referencias necesarias al iniciar el juego
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _player = GameObject.Find("Player").transform; // Busca al jugador en la escena
    }

    void Update()
    {
        // Calcula la distancia entre el enemigo y el jugador en cada frame
        _distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        // Si el jugador est� dentro del rango de movimiento
        if (_distanceToPlayer <= _distanceMoveToPlayer)
        {
            // Si el jugador est� a la izquierda y el enemigo mira a la derecha, girar
            if (_player.position.x < transform.position.x && _rightMovement)
            {
                Girar();
            }
            // Si el jugador est� a la derecha y el enemigo mira a la izquierda, girar
            if (_player.position.x > transform.position.x && !_rightMovement)
            {
                Girar();
            }
        }
    }

    void FixedUpdate()
    {
        // Si no hay suelo debajo, girar
        if (!Physics.Raycast(_floorFinder.position, Vector3.down, out RaycastHit _floor, _distanceLineDown, _floorLayer))
        {
            Girar();
        }

        // Si detecta una pared, girar (ignora al jugador gracias a la capa _wallLayer)
        if (Physics.Raycast(_floorFinder.position, Vector3.right, out RaycastHit _wallRight, _distanceLineRight, _wallLayer) ||
            Physics.Raycast(_floorFinder.position, Vector3.left, out RaycastHit _wallLeft, _distanceLineRight, _wallLayer))
        {
            Girar();
        }

        Move(); // Llama a la funci�n de movimiento
    }

    private void Move()
    {
        // Si el jugador est� lejos, moverse a la velocidad normal
        if (_distanceToPlayer >= _distanceMoveToPlayer)
        {
            _rb.linearVelocity = new Vector3(_actualSpeed, _rb.linearVelocity.y, _rb.linearVelocity.z);
        }

        // Si el jugador est� cerca, seguir movi�ndose
        if (_distanceToPlayer < _distanceMoveToPlayer)
        {
            _rb.linearVelocity = new Vector3(_actualSpeed, _rb.linearVelocity.y, _rb.linearVelocity.z);
        }

        // Si el jugador est� lo suficientemente cerca para atacar
        if (_distanceToPlayer < _attackDistance)
        {
            _actualSpeed = 0; // Detiene el movimiento
            _anim.SetTrigger("isAttack"); // Activa la animaci�n de ataque
            Invoke("Timer", _tiempoAtacando); // Espera antes de volver a moverse
        }
    }

    void Timer()
    {
        // Restaura la velocidad del enemigo despu�s del ataque
        _actualSpeed = _speed;
    }

    private void Girar()
    {
        // Funci�n para girar el enemigo 180� cuando detecta un obst�culo o necesita cambiar de direcci�n
        _rightMovement = !_rightMovement; // Cambia la direcci�n del movimiento
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0); // Rota el enemigo
        _actualSpeed *= -1; // Invierte la velocidad
        _speed *= -1; // Invierte la velocidad base
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        // Dibuja el raycast hacia abajo (para detectar el suelo)
        Gizmos.DrawLine(_floorFinder.transform.position, _floorFinder.transform.position + Vector3.down * _distanceLineDown);
        // Dibuja el raycast hacia la izquierda (para detectar paredes)
        Gizmos.DrawLine(_floorFinder.transform.position, _floorFinder.transform.position + Vector3.left * _distanceLineRight);
        // Dibuja el raycast hacia la derecha (para detectar paredes)
        Gizmos.DrawLine(_floorFinder.transform.position, _floorFinder.transform.position + Vector3.right * _distanceLineRight);

        // Dibuja esferas representando las zonas de detecci�n del enemigo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _distanceToPlayer); // Representa la distancia al jugador

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _distanceMoveToPlayer); // Zona en la que el enemigo comienza a moverse

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance); // Zona en la que el enemigo comienza a atacar
    }
}
