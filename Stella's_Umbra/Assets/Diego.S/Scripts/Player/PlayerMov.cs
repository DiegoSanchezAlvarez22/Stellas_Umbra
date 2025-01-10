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
    InputAction _jump;
    InputAction _superJump;
    InputAction _dash;
    InputAction _grabWall;
    InputAction _moveObj;

    [Header("Walk")]
    [SerializeField] private Vector3 _direction; //no debe ser serializable
    [SerializeField] private float _walkForce;

    [Header("Jump")]
    [SerializeField] private bool _canJump; //no debe ser serializable
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpsLeft;
    [SerializeField] private int _jumpsLeftMax;
    [SerializeField] private bool _inFloor;

    [Header("SuperJump")]
    [SerializeField] private float _minJumpForce = 5f;
    [SerializeField] private float _maxJumpForce = 20f;
    [SerializeField] private bool _superJumpActive;
    [SerializeField] private float _timeSuperJumpActive;
    [SerializeField] private int _superJumpsLeft;
    private bool _isJumping;
    private float _holdStartTime;

    [Header("Dash")]
    [SerializeField] private bool _canDash; //no debe ser serializable
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashTimeRechargeNeed;
    [SerializeField] private float _dashTimeRechargeCounter;
    private Vector3 dashDirection;
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float lastDashTime = -Mathf.Infinity;

    [Header("Wall")]
    [SerializeField] protected bool _inWall;
    private bool _canGrabWall;

    [Header("MoveObj")]
    private bool _canMoveObj;
    [SerializeField] private Transform _initialObjParent;
    private GameObject _objInMove;

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();

        _jump = _input.actions["Jump"];
        _dash = _input.actions["Dash"];
        _superJump = _input.actions["SuperJump"];
        _grabWall = _input.actions["GrabWall"];
        _moveObj = _input.actions["MoveObj"];
    }

    private void Update()
    {
        _direction = _input.actions["Walk"].ReadValue<Vector2>();

        if (isDashing)
        {
            dashTime += Time.deltaTime;
            if (dashTime >= dashDuration)
            {
                isDashing = false;
                _rb.linearVelocity = Vector3.zero; // Detiene el movimiento del dash
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 movimiento = _direction * _walkForce * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + movimiento);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _inFloor = true;
            _canJump = true;
            _jumpsLeft = _jumpsLeftMax;
        }

        if (collision.gameObject.CompareTag("Wall") && _inFloor == false)
        {
            _inWall = true;
            _canJump = true;
            _jumpsLeft = _jumpsLeftMax;
        }

        if (collision.gameObject.CompareTag("MovableObj"))
        {
            _canMoveObj = true;
            _objInMove = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("EnemyAir") || collision.gameObject.CompareTag("EnemyFloor"))
        {
            VidaJugador _vida;
            _vida = gameObject.GetComponent<VidaJugador>();
            _vida.PerderVida(1);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {   
            _inFloor = false;
            _canJump = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            _inWall = false;
            _canJump = false;
        }

        if (collision.gameObject.CompareTag("MovableObj"))
        {
            _canMoveObj = false;
            _objInMove = null;
        }
    }

    private void OnEnable()
    {
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
        if (_inFloor && _superJumpsLeft > 0 && _superJumpActive == true)
        {
            // Registra el momento en que se presiona la tecla
            _holdStartTime = (float)_callbackContext.startTime;
            _isJumping = true;
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

            _superJumpsLeft -= 1;
        }
    }

    public void Dash(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.started && Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            dashDirection = new Vector3(Input.GetAxis("Horizontal"),
            0, Input.GetAxis("Vertical")).normalized;
            if (dashDirection == Vector3.zero) dashDirection = transform.forward;

            _rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
            isDashing = true;
            dashTime = 0f;
            lastDashTime = Time.time;
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
        if (_inWall && !_inFloor)
        {
            _canJump = true;
            _jumpsLeft = _jumpsLeftMax;
        }
    }

    private void MoveObjPerformed(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.performed && _canMoveObj)
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
