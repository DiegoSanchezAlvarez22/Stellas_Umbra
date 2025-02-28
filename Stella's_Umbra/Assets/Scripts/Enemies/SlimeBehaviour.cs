using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyLifes))]
public class SlimeBehaviour : MonoBehaviour
{
    #region Variables
    
    [Header("Movement")] // ---------- CONFIGURACIÓN DEL MOVIMIENTO ----------
    [SerializeField] private bool _rightMovement; // Indica si el enemigo se mueve hacia la derecha
    [SerializeField] private float _speed; // Velocidad base del enemigo
    [HideInInspector] private float _actualSpeed; // Velocidad actual (puede modificarse en ciertas situaciones)
    [HideInInspector] private float _distanceLineDown = 1f; // Distancia del raycast para detectar el suelo
    [HideInInspector] private float _distanceLineRight = 0.1f; // Distancia del raycast para detectar paredes
    [SerializeField] private Transform _floorFinder; // Objeto vacío que se usa como origen del raycast hacia el suelo
    [SerializeField] private LayerMask _floorLayer; // Capa que representa el suelo
    [SerializeField] private LayerMask _wallLayer; // Capa que representa las paredes
    private Rigidbody _rb; // Referencia al Rigidbody del enemigo

    [Header("Attack")] // ---------- CONFIGURACIÓN DEL ATAQUE ----------
    [SerializeField] private int _contactDamage; // Daño que hace el enemigo al tocar al jugador
    [SerializeField] private int _attackDamage; // Daño que hace el enemigo con su ataque
    [SerializeField] private float _rechargeAttackTime; // Tiempo de recarga entre ataques
    [SerializeField] private float _distanceMoveToPlayer; // Distancia a la que el enemigo comienza a moverse hacia el jugador
    [SerializeField] private float _attackDistance; // Distancia a la que el enemigo comienza a atacar
    [SerializeField] private float _tiempoAtacando; // Tiempo que dura la animación de ataque
    [SerializeField] private GameObject _colliderAttack;
    private float _timeToNextAttack = 0f;
    private float _distanceToPlayer; // Distancia actual entre el enemigo y el jugador
    private Animator _anim; // Referencia al Animator para controlar animaciones
    private Transform _player; // Referencia al transform del jugador

    //Variables para la música
    bool isPlayingWalkSound = false;
    bool isPlayingAttackSound = false;

    private bool _canAttack = false;
    #endregion

    void Start()
    {
        // Obtener componentes y referencias necesarias al iniciar el juego
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _player = GameObject.Find("Player").transform; // Busca al jugador en la escena

        _actualSpeed = _speed;
        Invoke("EnableAttack", 2f);
    }

    void EnableAttack()
    {
        _canAttack = true;
    }

    void Update()
    {
        // Calcula la distancia entre el enemigo y el jugador en cada frame
        _distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        // Si el jugador está dentro del rango de movimiento
        if (_distanceToPlayer <= _distanceMoveToPlayer)
        {
            // Si el jugador está a la izquierda y el enemigo mira a la derecha, girar
            if (_player.position.x < transform.position.x && _rightMovement)
            {
                Girar();
            }
            // Si el jugador está a la derecha y el enemigo mira a la izquierda, girar
            if (_player.position.x > transform.position.x && !_rightMovement)
            {
                Girar();
            }
        }

        _timeToNextAttack += Time.deltaTime;
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

        Move(); // Llama a la función de movimiento
    }

    private void Move()
    {
        // Si el jugador está lejos, moverse a la velocidad normal
        if (_distanceToPlayer >= _distanceMoveToPlayer)
        {
            _rb.linearVelocity = new Vector3(_actualSpeed, _rb.linearVelocity.y, _rb.linearVelocity.z);

            //Solo suena el sonido de caminar si no se está reproduciendo
            if (_distanceToPlayer < 10f && !isPlayingWalkSound)
            {
                isPlayingWalkSound = true;
                AudioManagerBehaviour.instance.PlaySFX("Slime Walking");
                //Se reinicia después de 1.5s
                Invoke("ResetWalkSound", 1.5f);
            }
        }

        // Si el jugador está cerca, seguir moviéndose
        if (_distanceToPlayer < _distanceMoveToPlayer)
        {
            _rb.linearVelocity = new Vector3(_actualSpeed, _rb.linearVelocity.y, _rb.linearVelocity.z);
        }

        // Si el jugador está lo suficientemente cerca para atacar
        if (_distanceToPlayer < _attackDistance && _canAttack)
        {
            _actualSpeed = 0; // Detiene el movimiento
            _anim.SetTrigger("isAttack"); // Activa la animación de ataque
            Invoke("Timer", _tiempoAtacando); // Espera antes de volver a moverse

            if (_timeToNextAttack >= 5)
            {
                StartCoroutine(SlimeAttack());
            }

            //Solo suena el sonido de ataque si no se ha activado recientemente
            if (!isPlayingAttackSound)
            {
                isPlayingAttackSound = true;
                AudioManagerBehaviour.instance.PlaySFX("Gas Attack");
                // Se reinicia después de 1s
                Invoke("ResetAttackSound", 1f);
            }
        }
    }

    private IEnumerator SlimeAttack()
    {
        _colliderAttack.SetActive(true);
        yield return new WaitForSeconds(0.65f);
        _colliderAttack.SetActive(false);
        _timeToNextAttack = 0;
    }

    void ResetWalkSound()
    {
        isPlayingWalkSound = false;
    }

    void ResetAttackSound()
    {
        isPlayingAttackSound = false;
    }


    void Timer()
    {
        // Restaura la velocidad del enemigo después del ataque
        _actualSpeed = _speed;
    }

    private void Girar()
    {
        // Función para girar el enemigo 180° cuando detecta un obstáculo o necesita cambiar de dirección
        _rightMovement = !_rightMovement; // Cambia la dirección del movimiento
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

        // Dibuja esferas representando las zonas de detección del enemigo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _distanceToPlayer); // Representa la distancia al jugador

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _distanceMoveToPlayer); // Zona en la que el enemigo comienza a moverse

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance); // Zona en la que el enemigo comienza a atacar
    }
}
