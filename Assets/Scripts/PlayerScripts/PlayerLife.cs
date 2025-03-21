using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    #region Variables
    [SerializeField] int _requieredCrystals;    // Cantidad de items necesarios para aumentar vida
    [SerializeField] int _regenerateLife;     // heal
    [SerializeField] int _actualCrystalsTaken;  // Items actuales recogidos
    [SerializeField] int _actualLife;         // Vida inicial del jugador
    [SerializeField] int _maxHearts;         // Vida m�xima del jugador

    public UnityEvent<int> _changeLife;
    public UnityEvent<int> _increaseHeart;

    //Propiedades para conseguir la vida actual y vida m�xima para guardar la info
    public int ActualLife => _actualLife;
    public int MaxLife => _maxHearts;
    public int ActualCrystalsTaken => _actualCrystalsTaken;

    //Referencia al script de guardado de datos
    [SerializeField] private CheckPointSystem _checkPointSystem;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerMov _playerMov;
    [SerializeField] private PlayerAttacks _playerAttacks;
    [SerializeField] private EnemyLifes _enemyLifes;

    [SerializeField] private float _looseControlTime;

    public GameObject _deathCanvas;
    public GameObject _starsBackground;

    [SerializeField] HeartsUI _heartsUI;
    [SerializeField] RectTransform _uiHeartsCnavas;
    [SerializeField] float _moveCanvas = 250f;

    [Header("Icons")]
    [SerializeField] GameObject _heartsCanvas;
    [SerializeField] GameObject _mapDropdown;
    [SerializeField] GameObject _energyOrbIcon;
    [SerializeField] GameObject _interactIcon;
    [SerializeField] GameObject _bossHealthBar;
    [SerializeField] GameObject _enemyIndicator;
    #endregion 

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerMov = GetComponent<PlayerMov>();
        _playerAttacks = GetComponent<PlayerAttacks>();
        _enemyLifes = GetComponent<EnemyLifes>();

        _checkPointSystem.LoadProgress();
        _changeLife.Invoke(_actualLife);

        _deathCanvas.SetActive(false);
        _starsBackground.SetActive(false);
        _heartsCanvas.SetActive(true);
        _enemyIndicator.SetActive(false);
    }

    private void Update()
    {
        if (_actualLife <= 0)
        {
            if (_checkPointSystem != null && PlayerPrefs.HasKey("PlayerVida"))
            {
                _deathCanvas.SetActive(true);
                _heartsCanvas.SetActive(false);
                _mapDropdown.SetActive(false);
                _energyOrbIcon.SetActive(false);
                _interactIcon.SetActive(false);
                _bossHealthBar.SetActive(false);
                _starsBackground.SetActive(true);

                _playerAttacks._enemyIndicator.enabled = false;

                if (BossManager.Instance != null)
                {
                    BossManager.Instance.DestroyBossInstance();
                }

                Invoke("LoadProgressWithDeathCanvas", 1.5f);
                Debug.Log("El jugador ha muerto. Cargando el �ltimo progreso guardado.");
            }
            else if (!PlayerPrefs.HasKey("PlayerVida"))
            {
                SceneManager.LoadScene("Menu Principal");
                return;
            }
        }
    }

    //M�todo para que me cargue los datos guardados al morir, activando el canvas despues del tiempo del invoke
    private void LoadProgressWithDeathCanvas()
    {
        _checkPointSystem.LoadProgress();
        _enemyIndicator.SetActive(false);
        _heartsCanvas.SetActive(true);
        _mapDropdown.SetActive(true);
        AudioManagerBehaviour.instance.PlayAdventureMusic();
    }

    // M�todo para recoger un item
    public void TakeItem()
    {
        _actualCrystalsTaken++;
        Debug.Log("Item recogido. Total de items: " + _actualCrystalsTaken);

        // Verifica si ya tiene suficientes items para aumentar vida
        if (_actualCrystalsTaken >= _requieredCrystals)
        {
            _maxHearts = _maxHearts + 1; //Aumenta la vida max
            _actualLife = _actualLife + 1; //Aumenta la vida actual
            _actualCrystalsTaken = 0; //Resetea cristales guardados

            _increaseHeart.Invoke(_maxHearts); //Actualiza la UI
            _changeLife.Invoke(_actualLife); //Actualiza la vida actual en UI
            _heartsUI.MoreHearts(1); //A�ade 1 coraz�n a la UI

            Vector3 newPosition = _uiHeartsCnavas.localPosition;
            newPosition.x -= _moveCanvas; // Mueve en el eje X
            _uiHeartsCnavas.localPosition = newPosition;
            Debug.Log("Has recogido 4 items, tu vida: " + _maxHearts);
        }
    }

    // M�todo para aumentar la vida
    public void IncreaseLife(int regenerar)
    {
        if (_actualLife < _maxHearts)
        {
            _actualLife += regenerar;
            _changeLife.Invoke(_actualLife);
            Debug.Log("Vida aumentada. Vida actual: " + _actualLife);
        }
    }

    // M�todo para disminuir la vida
    public void LooseLife(int da�o, Vector2 _pos, string _tag)
    {
        StartCoroutine(DamageChangeColor());

        _actualLife -= da�o;

        if (_actualLife <= 0)
        {
            _actualLife = 0;

            AudioManagerBehaviour.instance.PlaySFX("Player Death");

            _enemyLifes.StopBossSoundLoop();

            Debug.Log("Has muerto");
        }
        else if (_actualLife > 0)
        {
            AudioManagerBehaviour.instance.PlaySFX("Player Hurt");

            if (_tag != "Boss")
            {
                StartCoroutine(LooseControl());
            }
            else
            {
                StartCoroutine(LooseControlBoss());
            }

            StartCoroutine(DisableCollision());

            if (_tag == "EnemyAir" || _tag == "EnemyFloor" || _tag == "Boss")
            {
                _playerMov.Bounce(_pos);
            }
        }

        _changeLife.Invoke(_actualLife);
        Debug.Log("Vida actual: " + _actualLife);
    }

    //Para poder guardar la info de la vida
    public void SetActualLife(int nuevaVida)
    {
        Debug.Log($"[SetActualLife] Intentando asignar vida: {nuevaVida}, Vida m�xima: {_maxHearts}");
        _actualLife = nuevaVida;
        Debug.Log($"[SetActualLife] Vida despu�s de asignaci�n: {_actualLife}");
        // Invocar evento para actualizar la UI
        _changeLife.Invoke(_actualLife);
    }

    public void SetMaxLife(int nuevaVidaMaxima)
    {
        _maxHearts = nuevaVidaMaxima;
        // Actualizar UI
        _increaseHeart.Invoke(_maxHearts);
    }

    //Para poder guardar la info de los cristales
    public void SetCrystalsNumber(int nuevaCantidadCristales)
    {
        _actualCrystalsTaken = nuevaCantidadCristales;
    }

    private IEnumerator LooseControl()
    {
        _playerMov._canMove = false;
        yield return new WaitForSeconds(_looseControlTime);
        _playerMov._canMove = true;
    }

    private IEnumerator LooseControlBoss()
    {
        _playerMov._canMove = false;
        yield return new WaitForSeconds(_looseControlTime / 2f);
        _playerMov._canMove = true;
    }

    private IEnumerator DisableCollision()
    {
        Physics.IgnoreLayerCollision(9, 10, true);
        yield return new WaitForSeconds(_looseControlTime);
        Physics.IgnoreLayerCollision(9, 10, false);
    }

    private IEnumerator DamageChangeColor()
    {
        Debug.Log("Cambio de color");
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        _spriteRenderer.color = Color.white;
    }

    void EnemyRespawn()
    {
        // Llamar a OnPlayerDeath() en cada enemigo registrado
        foreach (EnemyRespawn enemy in EnemyManager.Instance.GetEnemies())
        {
            enemy.OnPlayerDeath();
        }
    }
}