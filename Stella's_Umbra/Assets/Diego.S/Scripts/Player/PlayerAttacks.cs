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
    [SerializeField] public float _energy = 0; //energía actual del player
    [SerializeField] public float _energyBoost = 1; //aumenta la velocidad de obtención de energía

    [Header("BasicAttack")]
    [SerializeField] public bool _canBasicAttack; //variable a la que accede el skilltree
    [SerializeField] private GameObject _attackArea; //área en la que afecta el ataque
    [SerializeField] private float _timeAttaking = 0.2f; //tiempo durante el que hace daño el área
    [SerializeField] private float _timeRechargeAttack = 0.6f; //tiempo que tarda en recargarse el ataque
    private bool _isAttacking = false; //indica si el ataque se está realizando
    private bool _basicAttackRecharged = false; //indica si el ataque se ha recargado o no
    private float _timer = 0f; //tiempo desde que se comienza a atazar
    private float _timerRecharge = 0f; //tiempo desde que empieza a recargar el ataque

    [Header("BoulderAttack")]
    [SerializeField] public bool _canBoulderAttack; //variable a la que accede el skilltree
    [SerializeField] private GameObject _boulder; //prefab de roca que se instancia
    [SerializeField] private Transform shootingPoint; //posicion desde la que se dispara
    [SerializeField] private Vector3 _boulderSpawnRight; //posicion en la que se instancia la roca (derecha)
    [SerializeField] private Vector3 _boulderSpawnLeft; //posicion en la que se instancia la roca (izquierda)

    [Header("EnergyOrbAttack")]
    [SerializeField] public bool _canEnergyOrbAttack;
    [SerializeField] SphereCollider _detectionCollider;
    [SerializeField] Image _enemyIndicator;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _shootingPoint;
    [SerializeField] List<Collider> _enemiesInside = new List<Collider>(); //Lista para detectar a los enemigos dentro del Sphere Collider

    [Header("TornadoAttack")]
    [SerializeField] public bool _canTornadoAttack; //variable a la que accede el skilltree
    [SerializeField] private GameObject _tornado; //prefab de tornado que se instancia
    [SerializeField] private float _tornadoAttackDuration; //durancion del ataque especial
    //[SerializeField] private float _damage; //daño que hace el tornado
    //[SerializeField] private float _damageInterval; //cada cuánto tiempo hace daño
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

        #region BasicAttack
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

        #region EnergyOrbAttack

        if (_canEnergyOrbAttack)
        {
            //Desactiva el indicador del enemigo al estar el juego en pausa
            if (Time.timeScale == 0f)
            {
                _enemyIndicator.gameObject.SetActive(false);
                return;
            }

            //Muestra/Oculta la imagen si hay enemigos en la 1º posición o no
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyFloor") || other.CompareTag("EnemyAir") || other.CompareTag("Boss"))
        {
            //Añade al enemigo en la lista
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
        _basicAttack.performed += BasicAttack;

        _boulderAttack.Enable();
        _boulderAttack.started += BoulderAttackStarted;
        _boulderAttack.performed += BoulderAttack;

        _energyOrbAttack.Enable();
        _energyOrbAttack.performed += ShootBullet;

        _tornadoAttack.Enable();
        _tornadoAttack.performed += TornadoAttackStarted;
    }

    public void OnDisable()
    {
        _basicAttack.performed -= BasicAttack;
        _basicAttack.Disable();

        _boulderAttack.started -= BoulderAttackStarted;
        _boulderAttack.performed -= BoulderAttack;
        _boulderAttack.Disable();

        _energyOrbAttack.performed -= ShootBullet;
        _energyOrbAttack.Disable();

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
        if (_callbackContext.performed && _basicAttackRecharged)
        {
            if (_canBasicAttack)
            {
                _isAttacking = true;
                _attackArea.SetActive(_isAttacking);

                AudioManagerBehaviour.instance.PlaySFX("BasicAttack");

                _basicAttackRecharged = false;
                _timerRecharge = 0;
            }
        }
    }

    private void BoulderAttack(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.performed)
        {
            if (_canBoulderAttack == true)
            {
                AudioManagerBehaviour.instance.PlaySFX("BoulderAttack");

                if (_spriteRenderer.flipX == true)
                {
                    shootingPoint.localPosition = _boulderSpawnRight;
                    
                }
                else
                {
                    shootingPoint.localPosition = _boulderSpawnLeft;
                    
                }
                
                GameObject instantiatedRoca = GameObject.Instantiate
                    (_boulder, shootingPoint.position, shootingPoint.rotation);
            }
            
        }

    }

    private void BoulderAttackStarted(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.started)
        {
            if (_canBoulderAttack == true)
            {
                _anim.SetBool("BoulderAttack", true); //Isa
                StopBoulderAttackAnim();
            }
        }
    }

    void StopBoulderAttackAnim()
    {
        _anim.SetBool("BoulderAttack", false); //Isa      
    }

    private void PositionIndicator(Collider enemy)
    {
        //Convertir la posición del enemigo en una posición de la pantalla
        Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);

        //Ajusta la posición de la imagen un poco por encima
        screenPos.y += 110f;
        _enemyIndicator.transform.position = screenPos;
    }

    private void ShootBullet(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.performed)
        {
            if (_canEnergyOrbAttack == true)
            {
                if (_bulletPrefab != null && _shootingPoint != null && _enemiesInside.Count > 0)
                {
                    //Obtiene al primer enemigo de la lista
                    Transform targetEnemy = _enemiesInside[0].transform;

                    //Modifica el shootingPoint
                    _shootingPoint.localPosition = new Vector3(0, 0, 0);

                    //Calcula la dirección hacia el enemigo
                    Vector3 direction = (targetEnemy.position - _shootingPoint.position).normalized;

                    //Instancia la bala
                    GameObject bulletInstance = Instantiate(_bulletPrefab, _shootingPoint.position, _shootingPoint.rotation);

                    //Configura la dirección de la bala
                    BulletBehaviour _bulletBehaviour = bulletInstance.GetComponent<BulletBehaviour>();

                    if (_bulletBehaviour != null)
                    {
                        _bulletBehaviour.SetDirection(direction);
                    }

                    //Reproduce el sonido
                    AudioManagerBehaviour.instance.PlaySFX("EnergyOrbAttack");

                    Debug.Log("Se ha disparado la bala");
                }
                else
                {
                    Debug.LogWarning("Falta asignar el prefab de la bala o el shootingPoint al Inspector");
                }
            }
        }
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
        }
    }

    private IEnumerator TornadoAttack()
    {
        if (_canTornadoAttack == true)
        {
            if (_energy >= 100)
            {
                AudioManagerBehaviour.instance.PlaySFX("Tornado Attack");
                GameObject instantiatedTornado;
                instantiatedTornado = GameObject.Instantiate(_tornado, new Vector3(transform.localPosition.x, (float)2.5, transform.localPosition.z),
                    shootingPoint.rotation);
                instantiatedTornado.transform.SetParent(transform);
                yield return new WaitForSeconds(_tornadoAttackDuration);
                _energy = 0;
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
