using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMov : MonoBehaviour
{
    #region Variables

    [Header("Physics")]
    Rigidbody _rb;

    [Header("New Input System")]
    PlayerInput _input;
    InputAction _bendDown;
    InputAction _jump;
    InputAction _superJump;
    InputAction _dash;
    InputAction _grabWall;
    InputAction _moveObj;

    [Header("Walk")]
    [SerializeField] private float _walkForce;
    [SerializeField] private Vector2 _bounce; //rebote del player al recibir daño
    [HideInInspector] public bool _canMove = true;
    private Vector3 _direction; //no debe ser serializable
    private bool _wasWalkingBeforeFall = false; //Caminar antes de caer

    [Header("Bend Down")]
    [SerializeField] private GameObject _platDetector; //obj que detecta si se esta sobre una plataforma
    [HideInInspector] public Collider _otherCollider; //collider del objeto que se presupone es una plataforma
    [HideInInspector] public bool _floorIsPlat; //indica si el suelo es una plataforma
    private float _platDetectorTimer = 0f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce; //indica la potencia del salto
    [SerializeField] public float _jumpsLeft; //indica el numero de saltos que aun puede realizar
    [SerializeField] public int _jumpsLeftMax; //indica el numero de saltos que puede realizar
    [HideInInspector] private bool _inFloor; //indica si esta sobre el suelo
    [HideInInspector] public bool _canJump = true; //variable a la que accede el skilltree

    [Header("SuperJump")]
    [SerializeField] private float _minJumpForce = 5f; //fuerza mínima que se aplicará al supersalto
    [SerializeField] private float _maxJumpForce = 20f; //fuerza máxima que se aplicará al supersalto
    [SerializeField] private float _SuperJumpRechargeTime; //tiempo hasta que pueda volver a supersaltar
    [HideInInspector] public bool _canSuperJump; //variable a la que accede el skilltree
    [HideInInspector] private bool _superJumpRecharged; //indica si se ha recargado el supersalto
    [HideInInspector] private bool _finishedSuperJumping = false; //indica si ha terminado el supersalto
    private float _timer = 0; //timer para actualizar la recarga del supersalto
    private bool _isJumping; //indica si está saltando
    private float _holdStartTime; //tiempo durante el que se presiona la tecla o boton asignada al supersalto

    [Header("Dash")]
    [SerializeField] private float _dashForce; //fuerza con la que se ejecuta el dash
    [SerializeField] private float _dashTimeRechargeNeed; //tiempo necesario para que se recargue el dash
    [SerializeField] private float _dashTimeRechargeCounter; //timer para actualizar la recarga del dash
    [HideInInspector] public bool _canDash; //variable a la que accede el skilltree
    public float _dashDuration = 0.2f; //duración del dash
    public float _dashCooldown = 1f;
    private bool _isDashing = false;
    private float _dashTime = 0f;
    private float _lastDashTime = -Mathf.Infinity;
    private GameObject _trailRenderer; //rastro que deja detras de sí mísmo
    private Vector3 _dashDirection; //direccion en la que se realizará el dash

    [Header("Wall")]
    [HideInInspector] public bool _canWallJump; //variable a la que accede el skilltree
    protected bool _inWall; //indica si está en la pared
    private bool _canGrabWall; //indica si puede agarrarse a la pared

    [Header("MoveObj")]
    [SerializeField] private Transform _initialObjParent;
    [HideInInspector] public bool _canMoveObj;
    [HideInInspector] private bool _objCanBeMoved;
    private GameObject _objInMove;

    [Header("Animations")]
    [SerializeField] Animator _anim;

    [Header("Render")]
    [SerializeField] SpriteRenderer _spriteRenderer;

    [Header("Collider")]
    [SerializeField] BoxCollider _collider;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider>();

        _trailRenderer = GameObject.FindWithTag("PlayerTrail");

        _bendDown = _input.actions["BendDown"];
        _jump = _input.actions["Jump"];
        _dash = _input.actions["Dash"];
        _superJump = _input.actions["SuperJump"];
        _grabWall = _input.actions["GrabWall"];
        _moveObj = _input.actions["MoveObj"];
    }

    private void Update()
    {
        if (_canMove)
        {
            _direction = _input.actions["Walk"].ReadValue<Vector2>();
        }
        float speed = _direction.x;

        if (_platDetectorTimer > 0) //Reactivar el detector de plataformas después del tiempo definido
        {
            _platDetectorTimer -= Time.deltaTime;
            if (_platDetectorTimer <= 0)
            {
                _platDetector.SetActive(true);
            }
        }

        if (_isDashing)
        {
            _dashTime += Time.deltaTime;
            if (_dashTime >= _dashDuration)
            {
                _isDashing = false;
                _rb.linearVelocity = Vector3.zero; // Detiene el movimiento del dash
                _trailRenderer.GetComponent<TrailRenderer>().emitting = false; //activa la estela que sigue al player
            }
        }

        if (_finishedSuperJumping)
        {

            _timer += Time.deltaTime; // Sumar tiempo cada frame

            Debug.Log("Tiempo transcurrido: " + _timer); // Para depuración

            if (_timer >= _SuperJumpRechargeTime)
            {
                _superJumpRecharged = true;
                _finishedSuperJumping = false;
                _timer = 0; // Reiniciar el timer para futuros usos
            }
        }

        _anim.SetFloat("Movement", speed);

        if (_direction.x > 0)
        {
            _spriteRenderer.flipX = true; //Cambiamos la rotacion del pj
        }
        else if (_direction.x < 0)
        {
            _spriteRenderer.flipX = false;
        }

        //Verificar si está cayendo al saltar
        if (_rb.linearVelocity.y < -0.1f && !_inFloor)
        {
            _anim.SetBool("isFalling", true);
            _anim.SetBool("isJumping", false);
        }

        if (_inFloor || _floorIsPlat)
        {
            _wasWalkingBeforeFall = Mathf.Abs(_direction.x) > 0.1f;// Si se está moviendo en X, estaba caminando
        }
    }

    private void FixedUpdate()
    {
        if (_canMove)
        {
            Vector3 movimiento = _direction * _walkForce * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + movimiento);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _inFloor = true;
            _canJump = true;
            _jumpsLeft = _jumpsLeftMax;

            //Resetear las animaciones de salto y caída
            _anim.SetBool("isJumping", false);
            _anim.SetBool("isFalling", false);

            _wasWalkingBeforeFall = false;
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            _floorIsPlat = true;
            _canJump = true;
            _jumpsLeft = _jumpsLeftMax;

            //Resetear las animaciones de salto y caída
            _anim.SetBool("isJumping", false);
            _anim.SetBool("isFalling", false);

            _wasWalkingBeforeFall = false;
        }

        if (collision.gameObject.CompareTag("Wall") && _inFloor == false)
        {
            _inWall = true;
        }

        if (collision.gameObject.CompareTag("MovableObj"))
        {
            _objCanBeMoved = true;
            _objInMove = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
        {   
            _inFloor = false;
            _canJump = false;

            if (_wasWalkingBeforeFall && !_inWall) //Activar animación caída si caminaba
            {
                _anim.SetBool("isFalling", true);
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            _inWall = false;
            _canJump = false;
        }

        if (collision.gameObject.CompareTag("MovableObj"))
        {
            _objCanBeMoved = false;
            _objInMove = null;
        }
    }

    private void OnEnable()
    {
        _bendDown.performed += BendDownStarted;
        _bendDown.canceled += BendDownCanceled;

        _jump.Enable();
        _jump.performed += Jump;

        _superJump.Enable();
        _superJump.started += OnSuperJumpStarted;
        _superJump.canceled += OnSuperJumpCanceled;

        _dash.Enable();
        _dash.performed += Dash;

        _grabWall.Enable();
        _grabWall.performed += WallGrabPerformed;
        _grabWall.canceled += WallGrabCanceled;

        _moveObj.Enable();
        _moveObj.performed += MoveObjPerformed;
        _moveObj.canceled += MoveObjCanceled;
    }

    private void OnDisable()
    {
        _bendDown.performed -= BendDownStarted;
        _bendDown.canceled -= BendDownCanceled;

        _jump.performed -= Jump;
        _jump.Disable();

        _superJump.started -= OnSuperJumpStarted;
        _superJump.canceled -= OnSuperJumpCanceled;
        _superJump.Disable();

        _dash.performed -= Dash;
        _dash.Disable();

        _grabWall.performed -= WallGrabPerformed;
        _grabWall.performed -= WallGrabPerformed;
        _grabWall.Disable();

        _moveObj.performed -= MoveObjPerformed;
        _moveObj.canceled -= MoveObjCanceled;
        _moveObj.Disable();
    }

    public bool MovSkillsActivation(string _skillName, bool _learned)
    {
        if (_skillName == "Dash")
        {
            if (_learned)
            {
                _canDash = true;
                return true;
            }
            else
            {
                _canDash = false;
                return true;
            }
        }

        if (_skillName == "MoveObj")
        {
            if (_learned)
            {
                _canMoveObj = true;
                return true;
            }
            else
            {
                _canMoveObj = false;
                return true;
            }
        }

        if (_skillName == "WallJump")
        {
            if (_learned)
            {
                _canWallJump = true;
            }
            else
            {
                _canWallJump = false;
            }
        }

        if (_skillName == "DoubleJump" || _skillName == "TripleJump")
        {
            if (_learned)
            {
                _jumpsLeft += 1;
                _jumpsLeftMax += 1;
                return true;
            }
            else
            {
                _jumpsLeft -= 1;
                _jumpsLeftMax -= 1;
                return true;
            }
        }

        if (_skillName == "SuperJump")
        {
            if (_learned)
            {
                _canSuperJump = true;
                _superJumpRecharged = true;
                return true;
            }
            else
            {
                _canSuperJump = false;
                _superJumpRecharged = false;
                return true;
            }
        }

        return false;
    }

    public void Bounce(Vector2 _hitSpot)
    {
        _rb.linearVelocity = new Vector2 (-_bounce.x * _hitSpot.x, _bounce.y);
    }

    private void BendDown(Collider _collision, bool _isPlat)
    {
        if (!_isPlat && _inFloor)
        {
            _collider.center = new Vector3(0, -0.45f, 0);
            _collider.size = new Vector3(0.6808743f, 0.5f, 1);
            _walkForce = 5f;
        }
        else if (_isPlat)
        {
            _platDetector.SetActive(false); // Desactiva el detector de plataformas
            _collision.isTrigger = true;
            _platDetectorTimer = 0.5f; // Inicia el temporizador
        }
    }

    private void BendDownStarted(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.performed)
        {
            BendDown(_otherCollider, _floorIsPlat);
        }
    }

    private void BendDownCanceled(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.canceled)
        {
            _collider.center = new Vector3(0, -0.2f, 0);
            _collider.size = new Vector3(0.6808743f, 1, 1);
            _walkForce = 10f;
            _floorIsPlat = false;
        }
    }

    public void Jump(InputAction.CallbackContext _callbackContext)
    {
        WallJump();
        if (_callbackContext.performed)
        {
            ReviewJumpsInAir();
            if (_canJump)
            {
                _rb.AddForce(Vector3.up * _jumpForce * 10);
                _jumpsLeft = _jumpsLeft - 1f;

                //Se reinicia la velocidad vertical
                _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);

                //Animación se apaga antes de reiniciarla
                _anim.SetBool("isJumping", false);

                //Se reproduce la animación desde cero
                _anim.Play("LyraJump", -1, 0f);
                _anim.SetBool("isJumping", true);
                _anim.SetBool("isFalling", false);
            }
        }
    }

    private void ReviewJumpsInAir()
    {
        if (_jumpsLeft > 0)
        {
            _canJump = true;
        }
        else
        {
            _canJump = false;
        }
    }

    private void OnSuperJumpStarted(InputAction.CallbackContext _callbackContext)
    {
        if ((_inFloor || _floorIsPlat) && _superJumpRecharged && _canSuperJump)
        {
            // Registra el momento en que se presiona la tecla
            _holdStartTime = (float)_callbackContext.startTime;
            _isJumping = true;
            _superJumpRecharged = false;
        }
    }

    private void OnSuperJumpCanceled(InputAction.CallbackContext _callbackContext)
    {
        if (_isJumping)
        {
            // Calcula el tiempo que se mantuvo presionado el bot�n
            float _holdDuration = (float)(_callbackContext.time - _holdStartTime);

            // Calcula la fuerza del salto en funci�n de la duraci�n de la pulsaci�n
            float _superJumpForce = Mathf.Lerp(_minJumpForce, _maxJumpForce, _holdDuration);

            _rb.AddForce(Vector3.up * _superJumpForce, ForceMode.Impulse);

            Debug.Log("Salto realizado con una fuerza de: " + _superJumpForce);
            _isJumping = false;

            _finishedSuperJumping = true;

            //Reinicia la animación de salto (JULIO)
            _anim.SetBool("isJumping", false);
            _anim.Play("LyraJump", -1, 0f);
            _anim.SetBool("isJumping", true);
            _anim.SetBool("isFalling", false);
        }
    }

    public void Dash(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.started && Time.time >= _lastDashTime + _dashCooldown && !_isDashing && _canDash)
        {
            _dashDirection = new Vector3(Input.GetAxis("Horizontal"),
            0, Input.GetAxis("Vertical")).normalized;
            if (_dashDirection == Vector3.zero) _dashDirection = transform.forward;

            _trailRenderer.GetComponent<TrailRenderer>().emitting = true;

            _rb.AddForce(_dashDirection * _dashForce, ForceMode.Impulse);
            _isDashing = true;
            _dashTime = 0f;
            _lastDashTime = Time.time;
        }
    }

    private void WallGrabPerformed(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.performed && _inWall)
        {
            Debug.Log("En pared");
        }
    }

    private void WallGrabCanceled(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.canceled || !_inWall)
        {
            Debug.Log("ya no pared");
        }
    }

    private void WallJump()
    {
        if (_canWallJump && _inWall && !_inFloor)
        {
            _canJump = true;
            _jumpsLeft = _jumpsLeftMax;

            //Reinicia la animación de salto (JULIO)
            _anim.SetBool("isJumping", false);
            _anim.Play("LyraJump", -1, 0f);
            _anim.SetBool("isJumping", true);
            _anim.SetBool("isFalling", false);
        }
    }

    private void MoveObjPerformed(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.performed && _objCanBeMoved && _canMoveObj)
        {
            _canJump = false;
            _objInMove.transform.SetParent(this.gameObject.transform);
            _objInMove.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void MoveObjCanceled(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.canceled)
        {
            _objInMove.transform.SetParent(_initialObjParent);
            _objInMove.GetComponent<Rigidbody>().isKinematic = false;
            _canJump = true;
        }
    }
}
