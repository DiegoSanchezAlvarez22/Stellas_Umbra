using System.Collections;
using Unity.VisualScripting;
using System.Threading;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [Header("Sprite Renderer")]
    [SerializeField] SpriteRenderer _spriteRenderer;

    [Header("Energy")] //ambas variables deben ser guardadas
    [SerializeField] private float _energy = 0; //energía actual del player
    [SerializeField] private float _energyBoost = 0; //aumenta la velocidad de obtención de energía

    [Header("BasicAttack")]
    [SerializeField] private bool _canBasicAttack; //variable a la que accede el skilltree
    [SerializeField] private GameObject _attackArea; //área en la que afecta el ataque
    [SerializeField] private float _timeAttaking = 0.2f; //tiempo durante el que hace daño el área
    [SerializeField] private float _timeRechargeAttack = 0.6f; //tiempo que tarda en recargarse el ataque
    private bool _isAttacking = false; //indica si el ataque se está realizando
    private bool _basicAttackRecharged = false; //indica si el ataque se ha recargado o no
    private float _timer = 0f; //tiempo desde que se comienza a atazar
    private float _timerRecharge = 0f; //tiempo desde que empieza a recargar el ataque

    [Header("BoulderAttack")]
    [SerializeField] private bool _canBoulderAttack; //variable a la que accede el skilltree
    [SerializeField] private GameObject _boulder; //prefab de roca que se instancia
    [SerializeField] private Vector3 _boulderSpawnRight; //posicion en la que se instancia la roca (derecha)
    [SerializeField] private Vector3 _boulderSpawnLeft; //posicion en la que se instancia la roca (izquierda)

    [Header("TornadoAttack")]
    [SerializeField] private bool _canTornadoAttack; //variable a la que accede el skilltree
    [SerializeField] private GameObject _tornado; //prefab de tornado que se instancia
    [SerializeField] private float _tornadoAttackDuration; //durancion del ataque especial

    #region Variables Diego B

    private Vector3 shootingPointOriginal;
    private BoxCollider boxCollider;
    [SerializeField] float tamañoAgachado;

    [Header("Ataque a Distancia")]
    [SerializeField] private Transform shootingPoint; //posicion desde la que se dispara
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform fijador;
    [SerializeField] float alturaSobrePadre = 1.0f; // Altura deseada encima del nuevo padre
    private Transform fijador2;
    private Transform padreOriginal;
    private GameObject rangoDeApuntado;
    private Vector3 posicionOriginal;

    #endregion

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        #region Start Diego B
        fijador2 = fijador.GetChild(0);
        // Guardamos el padre y la posición original del objeto hijo
        posicionOriginal = fijador.localPosition;
        fijador2.GetComponent<Renderer>().enabled = false;
        padreOriginal = fijador.transform.parent;
        shootingPointOriginal = shootingPoint.localPosition;
        #endregion
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
        if (Input.GetKeyDown(KeyCode.G) && _basicAttackRecharged)
        {
            BasicAttack();
        }

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

        //DistanceShoot();

        if (Input.GetKeyDown(KeyCode.E))
        {
            UpShoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(TornadoAttack());
        }

        Agacharse();
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

    private void BasicAttack()
    {
        if (_canBasicAttack)
        {
            _isAttacking = true;
            _attackArea.SetActive(_isAttacking);

            _basicAttackRecharged = false;
            _timerRecharge = 0;
        }
    }

    private void UpShoot()
    {
        if (_canBoulderAttack == true)
        {
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

    private IEnumerator TornadoAttack()
    {
        if (_canTornadoAttack == true)
        {
            if (_energy >= 100)
            {
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

    #region Metodos Diego B

    ////Establecer Fijador
    //private void OnTriggerEnter(Collider sphereCollider)
    //{
    //    if (sphereCollider.CompareTag("Interactuable") || sphereCollider.CompareTag("EnemyAir") || sphereCollider.CompareTag("EnemyFloor"))
    //    {
    //        Debug.Log("El objeto ha entrado en el SphereCollider del hijo.");
    //        fijador.SetParent(sphereCollider.transform);
    //        Debug.Log("Fijador ahora es hijo de objeto");
    //        // Coloca el objeto hijo justo encima del nuevo padre
    //        fijador.localPosition = new Vector3(0, alturaSobrePadre, 0);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Interactuable") || other.CompareTag("EnemyAir") || other.CompareTag("EnemyFloor"))
    //    {
    //        Debug.Log("El objeto ha salido en el SphereCollider del hijo.");
    //        fijador.transform.parent = padreOriginal;
    //        fijador.localPosition = new Vector3(0, alturaSobrePadre, 0);
    //        Debug.Log("Fijador ya no es hijo de objeto");
    //    }
    //}

    ////Atque distancia
    //void DistanceShoot()
    //{
    //    // Hacer visible el objeto hijo mientras la tecla esté presionada
    //    if (Input.GetKey(KeyCode.Tab))
    //    {
    //        fijador2.GetComponent<Renderer>().enabled = true;
    //    }
    //    else
    //    {
    //        fijador2.GetComponent<Renderer>().enabled = false;
    //    }
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        Debug.Log("Disparando");
    //        GameObject instantiatedBullet;
    //        instantiatedBullet = GameObject.Instantiate(projectile, shootingPoint.position, shootingPoint.rotation);
    //        instantiatedBullet.GetComponent<Disparofijado>().SetFijador(fijador);
    //    }
    //}

    private void Agacharse()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * tamañoAgachado, transform.localScale.z);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            boxCollider.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //Atravesar plataformas
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("SueloInestable"))
    //    {
    //        if (Input.GetKeyDown(KeyCode.S))
    //        {
    //            Debug.Log("Bajar la plataforma");
    //            Physics.IgnoreCollision(collision.collider, boxCollider, true);
    //        }
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("SueloInestable"))
    //    {
    //        Debug.Log("Reactiva plataforma");
    //        // Reactiva las colisiones con el objeto
    //        Physics.IgnoreCollision(collision.collider, boxCollider, true);
    //    }
    //}
    #endregion
}
