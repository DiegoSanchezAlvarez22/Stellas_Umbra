using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using static UnityEngine.InputSystem.InputAction;

public class PlayerAttacks : MonoBehaviour
{
    #region Variables

    [Header("New Input System")]
    PlayerInput _input;
    InputAction _basicAttack;
    InputAction _boulderAttack;
    InputAction _energyOrbAttack;
    InputAction _tornadoAttack;

    [Header("Sprite Renderer")]
    [SerializeField] SpriteRenderer _spriteRenderer;

    [Header("Animator")]
    [SerializeField] Animator _anim;

    [Header("Energy")] //ambas variables deben ser guardadas
    [SerializeField] public float _energy = 0; //energ�a actual del player
    [SerializeField] public float _energyBoost = 1; //aumenta la velocidad de obtenci�n de energ�a

    [Header("BasicAttack")]
    [SerializeField] public bool _canBasicAttack; //variable a la que accede el skilltree
    [SerializeField] private GameObject _attackArea; //�rea en la que afecta el ataque
    [SerializeField] private float _timeAttaking = 0.2f; //tiempo durante el que hace da�o el �rea
    [SerializeField] private float _timeRechargeAttack = 0.6f; //tiempo que tarda en recargarse el ataque
    private bool _isAttacking = false; //indica si el ataque se est� realizando
    private bool _basicAttackRecharged = false; //indica si el ataque se ha recargado o no
    private float _timer = 0f; //tiempo desde que se comienza a atazar
    private float _timerRecharge = 0f; //tiempo desde que empieza a recargar el ataque

    [Header("BoulderAttack")]
    [SerializeField] public bool _canBoulderAttack; //variable a la que accede el skilltree
    [SerializeField] private GameObject _boulder; //prefab de roca que se instancia
    [SerializeField] private Transform shootingPoint; //posicion desde la que se dispara
    [SerializeField] private Vector3 _boulderSpawnRight; //posicion en la que se instancia la roca (derecha)
    [SerializeField] private Vector3 _boulderSpawnLeft; //posicion en la que se instancia la roca (izquierda)
    [SerializeField] private float _timeRechargeBoulderAttack = 1.0f; // Ajusta seg�n sea necesario
    private bool _boulderAttackRecharged = false;
    private float _timerRechargeBoulder = 0f;

    [Header("EnergyOrbAttack")]
    [SerializeField] public bool _canEnergyOrbAttack;
    [SerializeField] SphereCollider _detectionCollider;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _shootingPoint;
    [SerializeField] List<Collider> _enemiesInside = new List<Collider>(); //Lista para detectar a los enemigos dentro del Sphere Collider
    [SerializeField] private float _timeRechargeEnergyOrbAttack = 1.5f; // Ajusta seg�n sea necesario
    [HideInInspector] public Image _enemyIndicator;
    private bool _energyOrbAttackRecharged = false;
    private float _timerRechargeEnergyOrb = 0f;

    [Header("TornadoAttack")]
    [SerializeField] public bool _canTornadoAttack; //variable a la que accede el skilltree
    [SerializeField] private GameObject _tornado; //prefab de tornado que se instancia
    [SerializeField] private float _tornadoAttackDuration; //durancion del ataque especial
    private bool _tornadoAttackRecharged;
    //[SerializeField] private float _damage; //da�o que hace el tornado
    //[SerializeField] private float _damageInterval; //cada cu�nto tiempo hace da�o
    //private List<EnemyLifes> enemiesInTornado = new List<EnemyLifes>();
    #endregion

    void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();

        _basicAttack = _input.actions["BasicAttack"];
        _boulderAttack = _input.actions["BoulderAttack"];
        _energyOrbAttack = _input.actions["EnergyOrbAttack"];
        _tornadoAttack = _input.actions["TornadoAttack"];
    }

    void Start()
    {
        _enemyIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        #region Energy
        if (_energy <= 100) //100 es el maximo conseguible
        {
            _energy = (_energy + Time.deltaTime) * _energyBoost;
        }
        #endregion

        #region BasicAttack Recharge
        if (_isAttacking)
        {
            _timer += Time.deltaTime;

            if (_timer >= _timeAttaking)
            {
                _timer = 0;
                _isAttacking = false;
                _attackArea.SetActive(_isAttacking);
            }
        }

        if (!_isAttacking)
        {
            _timerRecharge += Time.deltaTime;

            if (_timerRecharge >= _timeRechargeAttack)
            {
                _basicAttackRecharged = true;
            }
        }
        #endregion

        #region BoulderAttack Recharge
        if (!_boulderAttackRecharged)
        {
            _timerRechargeBoulder += Time.deltaTime;
            if (_timerRechargeBoulder >= _timeRechargeBoulderAttack)
            {
                _boulderAttackRecharged = true;
            }
        }
        #endregion

        #region EnergyOrbAttack Recharge
        if (!_energyOrbAttackRecharged)
        {
            _timerRechargeEnergyOrb += Time.deltaTime;
            if (_timerRechargeEnergyOrb >= _timeRechargeEnergyOrbAttack)
            {
                _energyOrbAttackRecharged = true;
            }
        }
        #endregion

        #region EnergyOrbAttack

        if (_canEnergyOrbAttack)
        {
            //Desactiva el indicador del enemigo al estar el juego en pausa
            if (Time.timeScale == 0f)
            {
                _enemyIndicator.gameObject.SetActive(false);
                return;
            }

            //Muestra/Oculta la imagen si hay enemigos en la 1� posici�n o no
            if (_enemiesInside.Count > 0)
            {
                //Habilita la imagen
                _enemyIndicator.gameObject.SetActive(true);

                //Posiciona la imagen en la pantalla encima del enemigo
                PositionIndicator(_enemiesInside[0]);

                //Detectar si pulsas la tecla C
                //if (Input.GetKeyDown(KeyCode.C))
                //{
                //    Debug.Log("Has pulsado la tecla C");
                //    ShootBullet();
                //}
            }
            else
            {
                //Deshabilita la imagen
                _enemyIndicator.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Tornado Recharge
        if (_energy >= 100)
        {
            _tornadoAttackRecharged = true;
        }
        else
        {
            _tornadoAttackRecharged = false;
        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyFloor") || other.CompareTag("EnemyAir") || other.CompareTag("Boss"))
        {
            //A�ade al enemigo en la lista
            _enemiesInside.Add(other);

            Debug.Log("Ha entrado un enemigo: " + other.gameObject.name);
            Debug.Log("Total de enemigos dentro: " + _enemiesInside.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyFloor") || other.CompareTag("EnemyAir") || other.CompareTag("Boss"))
        {
            //Elimina al enemigo de la lista
            _enemiesInside.Remove(other);

            Debug.Log("Ha salido un enemigo: " + other.gameObject.name);
            Debug.Log("Total de enemigos dentro: " + _enemiesInside.Count);
        }
    }

    public void OnEnable()
    {
        _basicAttack.Enable();
        _basicAttack.started += BasicAttackStarted;
        _basicAttack.performed += BasicAttack;

        _boulderAttack.Enable();
        _boulderAttack.started += BoulderAttackStarted;
        _boulderAttack.performed += BoulderAttack;

        _energyOrbAttack.Enable();
        _energyOrbAttack.started += ShootBulletStarted;
        _energyOrbAttack.performed += ShootBullet;

        _tornadoAttack.Enable();
        _tornadoAttack.performed += TornadoAttackStarted;
    }

    public void OnDisable()
    {
        _basicAttack.started -= BasicAttackStarted;
        _basicAttack.performed -= BasicAttack;
        _basicAttack.Disable();

        _boulderAttack.started -= BoulderAttackStarted;
        _boulderAttack.performed -= BoulderAttack;
        _boulderAttack.Disable();

        _energyOrbAttack.started -= ShootBulletStarted;
        _energyOrbAttack.performed -= ShootBullet;
        _energyOrbAttack.Disable();

        _tornadoAttack.started -= TornadoAttackStarted;
        _tornadoAttack.performed -= TornadoAttackStarted;
        _tornadoAttack.Disable();
    }

    public bool AttackSkillsActivation(string _skillName, bool _learned)
    {
        if (_skillName == "EnergyBoost1" || _skillName == "EnergyBoost2")
        {
            if (_learned)
            {
                _energyBoost += 1.005f;
            }
            else
            {
                _energyBoost -= 1.005f;
            }
            return true;
        }

        if (_skillName == "EnergyBoost3")
        {
            if (_learned)
            {
                _energyBoost += 1.01f;
            }
            else
            {
                _energyBoost -= 1.01f;
            }
        }

        if (_skillName == "BasicAttack")
        {
            if (_learned)
            {
                _canBasicAttack = true;
            }
            else
            {
                _canBasicAttack = false;
            }
            return true;
        }

        if (_skillName == "BoulderAttack")
        {
            if (_learned)
            {
                _canBoulderAttack = true;
            }
            else
            {
                _canBoulderAttack = false;
            }
            return true;
        }

        if (_skillName == "EnergyOrbAttack")
        {
            if (_learned)
            {
                _canEnergyOrbAttack = true;
            }
            else
            {
                _canEnergyOrbAttack = false;
            }
            return true;
        }

        if (_skillName == "TornadoAttack")
        {
            if (_learned)
            {
                _canTornadoAttack = true;
            }
            else
            {
                _canTornadoAttack = false;
            }
            return true;
        }

        return false;
    }

    private void BasicAttack(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.performed && _basicAttackRecharged == true)
        {
            if (_canBasicAttack)
            {
                _isAttacking = true;
                _attackArea.SetActive(_isAttacking);

                AudioManagerBehaviour.instance.PlaySFX("BasicAttack");

                _anim.SetBool("BasicAttack", true); //Isa

                _basicAttackRecharged = false;
                _timerRecharge = 0;
            }
        }
    }
    private void BasicAttackStarted(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.started)
        {
            if (_canBasicAttack == true && _basicAttackRecharged == true)
            {
                _anim.SetBool("BasicAttack", true); //Isa
            }
        }
    }

    void StopBasicAttackAnim()
    {
        _anim.SetBool("BasicAttack", false); //Isa      
    }

    private void BoulderAttack(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.performed && _boulderAttackRecharged)
        {
            if (_canBoulderAttack)
            {
                AudioManagerBehaviour.instance.PlaySFX("BoulderAttack");

                if (_spriteRenderer.flipX)
                {
                    shootingPoint.localPosition = _boulderSpawnRight;
                }
                else
                {
                    shootingPoint.localPosition = _boulderSpawnLeft;
                }

                GameObject instantiatedRoca = Instantiate(_boulder, shootingPoint.position, shootingPoint.rotation);

                // Reiniciar la recarga
                _boulderAttackRecharged = false;
                _timerRechargeBoulder = 0;
            }
        }
    }

    private void BoulderAttackStarted(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.started)
        {
            if (_canBoulderAttack == true && _boulderAttackRecharged)
            {
                _anim.SetBool("BoulderAttack", true); //Is
            }
        }
    }

   void StopBoulderAttackAnim()
   {
        _anim.SetBool("BoulderAttack", false); //Isa      
   }

    private void PositionIndicator(Collider enemy)
    {
        //Convertir la posici�n del enemigo en una posici�n de la pantalla
        Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);

        //Ajusta la posici�n de la imagen un poco por encima
        screenPos.y += 110f;
        _enemyIndicator.transform.position = screenPos;
    }

    private void ShootBullet(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.performed && _energyOrbAttackRecharged)
        {
            if (_canEnergyOrbAttack && _bulletPrefab != null && _shootingPoint != null && _enemiesInside.Count > 0)
            {
                Transform targetEnemy = _enemiesInside[0].transform;
                _shootingPoint.localPosition = new Vector3(0, 0, 0);

                Vector3 direction = (targetEnemy.position - _shootingPoint.position).normalized;

                GameObject bulletInstance = Instantiate(_bulletPrefab, _shootingPoint.position, _shootingPoint.rotation);
                BulletBehaviour _bulletBehaviour = bulletInstance.GetComponent<BulletBehaviour>();

                if (_bulletBehaviour != null)
                {
                    _bulletBehaviour.SetDirection(direction);
                }

                AudioManagerBehaviour.instance.PlaySFX("EnergyOrbAttack");

                // Reiniciar la recarga
                _energyOrbAttackRecharged = false;
                _timerRechargeEnergyOrb = 0;

                Debug.Log("Se ha disparado la bala");
            }
            else
            {
                Debug.LogWarning("Falta asignar el prefab de la bala o el shootingPoint al Inspector");
            }
        }
    }

    private void ShootBulletStarted(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.started)
        {
            if (_canEnergyOrbAttack == true && _energyOrbAttackRecharged)
            {
                _anim.SetBool("EnergyOrbAttack", true); //Isa

            }
        }
    }

    void StopShootBulletAnim()
    {
        _anim.SetBool("EnergyOrbAttack", false); //Isa      
    }
    public void RemoveEnemyFromList(GameObject enemy)
    {
        //Buscar el collider del enemigo en la lista y eliminarlo
        for (int i = 0; i < _enemiesInside.Count; i++)
        {
            if (_enemiesInside[i].gameObject == enemy)
            {
                _enemiesInside.RemoveAt(i);
                break;
            }
        }

        //Si no quedan enemigos, ocultar la imagen
        if (_enemiesInside.Count == 0)
        {
            _enemyIndicator.gameObject.SetActive(false);
        }
    }

    private void TornadoAttackStarted(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.performed)
        {
            StartCoroutine(TornadoAttack());
            if (_canTornadoAttack == true)
            {
                _anim.SetBool("TornadoAttack", true); //Isa
            }
        }
    }
   
    void StopTornadoAttackAnim()
    {
        _anim.SetBool("TornadoAttack", false); //Isa      
    }

    private IEnumerator TornadoAttack()
    {
        if (_canTornadoAttack == true && _tornadoAttackRecharged)
        {
            if (_energy >= 100)
            {
                _energy = 0;
                AudioManagerBehaviour.instance.PlaySFX("Tornado Attack");
                GameObject instantiatedTornado;
                instantiatedTornado = GameObject.Instantiate(_tornado, new Vector3(transform.localPosition.x, (float)2.5, transform.localPosition.z),
                    shootingPoint.rotation);
                instantiatedTornado.transform.SetParent(transform);
                yield return new WaitForSeconds(_tornadoAttackDuration);
                Destroy(instantiatedTornado);
            }
            else
            {
                Debug.Log("Energia insuficiente");
                //meter sonido como de error
            }
        }
    }
}
