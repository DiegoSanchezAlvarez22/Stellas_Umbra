using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] int _requieredCrystals;    // Cantidad de items necesarios para aumentar vida
    [SerializeField] int _regenerateLife;     // heal
    [SerializeField] int _actualCrystalsTaken;  // Items actuales recogidos
    [SerializeField] int _actualLife;         // Vida inicial del jugador
    [SerializeField] int _maxHearts;         // Vida máxima del jugador

    public UnityEvent<int> _changeLife;
    public UnityEvent<int> _increaseHeart;

    //JULIO Propiedades para conseguir la vida actual y vida máxima para guardar la info
    public int ActualLife => _actualLife;
    public int MaxLife => _maxHearts;
    public int ActualCrystalsTaken => _actualCrystalsTaken;

    //JULIO Referencia al script de guardado de datos
    [SerializeField] private CheckPointSystem _checkPointSystem;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerMov _playerMov;

    [SerializeField] private float _looseControlTime;

    public GameObject _deathCanvas;
    public GameObject _starsBackground;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerMov = GetComponent<PlayerMov>();

        _actualLife = _maxHearts;
        _changeLife.Invoke(_actualLife);

        _deathCanvas.SetActive(false);
        _starsBackground.SetActive(false);
    }

    private void Update()
    {
        if (_actualLife == 0)
        {
            //Respawn _respawn;
            //_respawn = gameObject.GetComponent<Respawn>();
            //_respawn.RespawnPosition();
            //vidaActual = corazonesMax;
            //cambioVida.Invoke(vidaActual);

            //CorazonesUI _corazonesUI;
            //_corazonesUI = gameObject.GetComponent<CorazonesUI>();
            //cambioVida.AddListener(_corazonesUI.CambiarCorazones);
            //sumarCorazon.AddListener(_corazonesUI.SumarCorazones);

            //JULIO Cuando la vida llege a 0, cargar el último progreso guardado
            //Si muere y tiene alguna Key guardada, que cargue la info guardada
            //Si muere y no tiene una Key guardada, que se reinicie la escena

            if (_actualLife == 0)
            {
                if (_checkPointSystem != null)
                {
                    _deathCanvas.SetActive(true);
                    _starsBackground.SetActive(true);
                    Invoke("LoadProgressWithDeathCanvas", 1.5f);
                    Debug.Log("El jugador ha muerto. Cargando el último progreso guardado.");
                }
                else
                {
                    Debug.Log("No se asignó el sistema de checkpoints al jugador.");
                }
            }
        }
    }

    //Método para que me cargue los datos guardados al morir, activando el canvas despues del tiempo del invoke
    private void LoadProgressWithDeathCanvas()
    {
        _checkPointSystem.LoadProgress();
    }

    // Método para recoger un item
    public void TakeItem()
    {
        _actualCrystalsTaken++;
        Debug.Log("Item recogido. Total de items: " + _actualCrystalsTaken);

        // Verifica si ya tiene suficientes items para aumentar vida
        if (_actualCrystalsTaken >= _requieredCrystals)
        {
            _maxHearts = _maxHearts + 1;
            _actualCrystalsTaken = 0;
            _increaseHeart.Invoke(_maxHearts);
        }
    }

    // Método para aumentar la vida
    public void IncreaseLife(int regenerar)
    {
        if (_actualLife < _maxHearts)
        {
            _actualLife += regenerar;
            _changeLife.Invoke(_actualLife);
            Debug.Log("Vida aumentada. Vida actual: " + _actualLife);
        }
    }

    // Método para disminuir la vida
    public void LooseLife(int daño, Vector2 _pos)
    {
        StartCoroutine(DamageChangeColor());

        _actualLife -= daño;

        if (_actualLife <= 0)
        {
            _actualLife = 0;
            Debug.Log("Has muerto");
        }
        else if (_actualLife > 0)
        {
            StartCoroutine(LooseControl());
            StartCoroutine(DisableCollision());
            _playerMov.Bounce(_pos);
        }

        _changeLife.Invoke(_actualLife);
        Debug.Log("Vida actual: " + _actualLife);

        
    }

    //JULIO Para poder guardar la info de la vida
    public void SetActualLife(int nuevaVida)
    {
        _actualLife = nuevaVida;
        // Invocar evento para actualizar la UI
        _changeLife.Invoke(_actualLife);
    }

    public void SetMaxLife(int nuevaVidaMaxima)
    {
        _maxHearts = nuevaVidaMaxima;
        // Actualizar UI
        _increaseHeart.Invoke(_maxHearts);
    }

    //JULIO Para poder guardar la info de los cristales
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
}