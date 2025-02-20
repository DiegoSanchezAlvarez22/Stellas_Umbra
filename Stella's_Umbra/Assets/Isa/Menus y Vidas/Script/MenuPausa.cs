using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.InputAction;

public class MenuPausa : MonoBehaviour
{
    private AudioManagerBehaviour _audioManagerBehaviour;

    [Header("Input")]
    private PlayerInput _input;
    private InputAction _openPauseMenu;

    [Header("Scenes")]
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject menuVidas;
    [SerializeField] private GameObject fondoPausa;
    [SerializeField] private GameObject menuOpciones;
    [SerializeField] private GameObject menuTeclado;
    [SerializeField] private GameObject menuMando;
    [SerializeField] private GameObject menuAjustes;
    [SerializeField] private GameObject brillo;
    [SerializeField] private GameObject puntero;

    private bool juegoPausado = false;
    private string lastControlScheme;

    [SerializeField] GameObject _botonMapa;
    [SerializeField] MapaDesplegable _mapaDesplegable;
    //Guardar estado del mapa
    bool mapaEstabaAbierto = false;


    private void Awake()
    {
        // Buscar el PlayerInput y validar si se encontró
        _input = FindAnyObjectByType<PlayerInput>();
        if (_input == null)
        {
            Debug.LogError("PlayerInput no encontrado en la escena.");
            return;
        }

        // Obtener la acción correcta desde el Input System
        _openPauseMenu = _input.actions["MenuActivation"];

        // Buscar el AudioManager
        _audioManagerBehaviour = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManagerBehaviour>();
    }

    private void Start()
    {
        // Inicializar la visibilidad de los menús
        menuPausa.SetActive(false);
        menuVidas.SetActive(true);
        fondoPausa.SetActive(false);
        menuOpciones.SetActive(false);
        menuTeclado.SetActive(false);
        menuMando.SetActive(false);
        menuAjustes.SetActive(false);
        brillo.SetActive(false);

        // Pausar la escena si es el "Menu Principal"
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Menu Principal")
        {
            Time.timeScale = 0;
            puntero.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            puntero.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _openPauseMenu.Enable();
        _openPauseMenu.performed += TogglePausa;  // Ahora usamos solo un método
    }

    private void OnDisable()
    {
        _openPauseMenu.performed -= TogglePausa;
        _openPauseMenu.Disable();
    }

    private void TogglePausa(InputAction.CallbackContext _callbackContext)
    {
        if (!_callbackContext.performed) return;

        if (juegoPausado)
        {
            Debug.Log("Reanudando...");
            juegoPausado = false;
            Time.timeScale = 1f;
            menuPausa.SetActive(false);
            menuVidas.SetActive(true);
            fondoPausa.SetActive(false);
            menuOpciones.SetActive(false);
            menuTeclado.SetActive(false);
            menuMando.SetActive(false);
            menuAjustes.SetActive(false);
            brillo.SetActive(false);
            puntero.SetActive(false);
            _botonMapa.SetActive(true);

            //Si el mapa estaba abierto antes de pausar, lo volvemos a abrir
            if (mapaEstabaAbierto)
            {
                _mapaDesplegable.mapaAnimator.Play("MapaAbierto");
            }
        }
        else
        {
            Debug.Log("Pausando...");
            juegoPausado = true;
            Time.timeScale = 0f;
            menuPausa.SetActive(true);
            menuVidas.SetActive(false);
            fondoPausa.SetActive(true);
            menuOpciones.SetActive(false);
            menuTeclado.SetActive(false);
            menuMando.SetActive(false);
            menuAjustes.SetActive(false);
            brillo.SetActive(true);
            puntero.SetActive(true);
            _botonMapa.SetActive(false);

            //Guarda si el mapa estaba abierto antes de pausar
            mapaEstabaAbierto = _mapaDesplegable.estaAbierto;
            if (mapaEstabaAbierto)
            {
                //Cierra el mapa
                _mapaDesplegable.mapaAnimator.Play("MapaCerrado");
            }
        }
    }

    public void Reanudar()
    {
        Debug.Log("Reanudando...");
        juegoPausado = false;
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
        menuVidas.SetActive(true);
        fondoPausa.SetActive(false);
        menuOpciones.SetActive(false);
        menuTeclado.SetActive(false);
        menuMando.SetActive(false);
        menuAjustes.SetActive(false);
        brillo.SetActive(false);
        puntero.SetActive(false);

        _botonMapa.SetActive(true);

        //Si el mapa estaba abierto antes de pausar, lo volvemos a abrir
        if (mapaEstabaAbierto)
        {
            _mapaDesplegable.mapaAnimator.Play("MapaAbierto");
        }
    }

    public void PlaySound()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
    }

    public void Opciones()
    {
        menuPausa.SetActive(false);
        menuOpciones.SetActive(true);
        brillo.SetActive(true);
        Time.timeScale = 0f;
    }

    public void VolverPausa()
    {
        menuPausa.SetActive(true);
        menuOpciones.SetActive(false);
        brillo.SetActive(true);
        Time.timeScale = 0f;
    }

    public void VolverOpciones()
    {
        menuOpciones.SetActive(true);
        menuTeclado.SetActive(false);
        menuMando.SetActive(false);
        menuAjustes.SetActive(false);
        brillo.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Teclado()
    {
        menuOpciones.SetActive(false);
        menuTeclado.SetActive(true);
        brillo.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Mando()
    {
        menuOpciones.SetActive(false);
        menuMando.SetActive(true);
        brillo.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Ajustes()
    {
        menuOpciones.SetActive(false);
        menuAjustes.SetActive(true);
        brillo.SetActive(true);
        Time.timeScale = 0f;
    }
}
