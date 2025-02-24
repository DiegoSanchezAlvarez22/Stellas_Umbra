using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.InputAction;

public class PauseMenu : MonoBehaviour
{
    private AudioManagerBehaviour _audioManagerBehaviour;

    [Header("Input")]
    private PlayerInput _input;
    private InputAction _openPauseMenu;

    [Header("Scenes")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _livesMenu;
    [SerializeField] private GameObject _backgroundPause;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _keyboardMenu;
    [SerializeField] private GameObject _gamepadMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _brightness;
    [SerializeField] private GameObject puntero;

    private bool pausedGame = false;
    private string lastControlScheme;

    [Header("Icons")]
    [SerializeField] GameObject _mapButton;
    [SerializeField] MapBehaviour _dropdownMap;
    [SerializeField] InteractionMesage _interactionMesage;

    //Guardar estado del mapa
    bool mapWasOpen = false;

    #region InputActionsDisabled
    private InputAction _walk;
    private InputAction _bendDown;
    private InputAction _jump;
    private InputAction _superJump;
    private InputAction _dash;
    private InputAction _moveObj;
    private InputAction _basicAttack;
    private InputAction _boulderAttack;
    private InputAction _energyOrbAttack;
    private InputAction _tornadoAttack;
    private InputAction _interact;
    private InputAction _activateSkilltree;
    #endregion

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
        _pauseMenu.SetActive(false);
        _livesMenu.SetActive(true);
        _backgroundPause.SetActive(false);
        _optionsMenu.SetActive(false);
        _keyboardMenu.SetActive(false);
        _gamepadMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _brightness.SetActive(false);

        // Establecer cuales son las acciones del input
        _walk = _input.actions["Walk"];
        _bendDown = _input.actions["BendDown"];
        _jump = _input.actions["Jump"];
        _superJump = _input.actions["SuperJump"];
        _dash = _input.actions["Dash"];
        _moveObj = _input.actions["MoveObj"];
        _interact = _input.actions["Interact"];
        _basicAttack = _input.actions["BasicAttack"];
        _boulderAttack = _input.actions["BoulderAttack"];
        _energyOrbAttack = _input.actions["EnergyOrbAttack"];
        _tornadoAttack = _input.actions["TornadoAttack"];

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

    private void DisableSkills()
    {
        _walk.Disable();
        _bendDown.Disable();
        _jump.Disable();
        _superJump.Disable();
        _dash.Disable();
        _moveObj.Disable();
        _interact.Disable();
        _basicAttack.Disable();
        _boulderAttack.Disable();
        _energyOrbAttack.Disable();
        _tornadoAttack.Disable();
    }
    private void Enable()
    {
        _walk.Enable();
        _bendDown.Enable();
        _jump.Enable();
        _superJump.Enable();
        _dash.Enable();
        _moveObj.Enable();
        _interact.Enable();
        _basicAttack.Enable();
        _boulderAttack.Enable();
        _energyOrbAttack.Enable();
        _tornadoAttack.Enable();
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

        if (pausedGame)
        {
            Debug.Log("Reanudando...");
            Enable();
            pausedGame = false;
            Time.timeScale = 1f;
            _pauseMenu.SetActive(false);
            _livesMenu.SetActive(true);
            _backgroundPause.SetActive(false);
            _optionsMenu.SetActive(false);
            _keyboardMenu.SetActive(false);
            _gamepadMenu.SetActive(false);
            _settingsMenu.SetActive(false);
            _brightness.SetActive(false);
            puntero.SetActive(false);
            _mapButton.SetActive(true);
            _interactionMesage.OnGameResumed();

            //Si el mapa estaba abierto antes de pausar, lo volvemos a abrir
            if (mapWasOpen)
            {
                _dropdownMap._anim.Play("MapaAbierto");
            }
        }
        else
        {
            Debug.Log("Pausando...");
            DisableSkills();
            pausedGame = true;
            Time.timeScale = 0f;
            _pauseMenu.SetActive(true);
            _livesMenu.SetActive(false);
            _backgroundPause.SetActive(true);
            _optionsMenu.SetActive(false);
            _keyboardMenu.SetActive(false);
            _gamepadMenu.SetActive(false);
            _settingsMenu.SetActive(false);
            _brightness.SetActive(true);
            puntero.SetActive(true);
            _mapButton.SetActive(false);
            _interactionMesage.OnGamePaused();

            //Guarda si el mapa estaba abierto antes de pausar
            mapWasOpen = _dropdownMap._isOpen;
            if (mapWasOpen)
            {
                //Cierra el mapa
                _dropdownMap._anim.Play("MapaCerrado");
            }
        }
    }

    public void Continue()
    {
        Debug.Log("Reanudando...");
        Enable();
        pausedGame = false;
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
        _livesMenu.SetActive(true);
        _backgroundPause.SetActive(false);
        _optionsMenu.SetActive(false);
        _keyboardMenu.SetActive(false);
        _gamepadMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _brightness.SetActive(false);
        puntero.SetActive(false);
        _mapButton.SetActive(true);
        _interactionMesage.OnGameResumed();

        //Si el mapa estaba abierto antes de pausar, lo volvemos a abrir
        if (mapWasOpen)
        {
            _dropdownMap._anim.Play("MapaAbierto");
        }
    }

    public void PlaySound()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
    }

    public void Options()
    {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
        _brightness.SetActive(true);
        Time.timeScale = 0f;
    }

    public void BackToMainMenu()
    {
        _pauseMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        _brightness.SetActive(true);
        Time.timeScale = 0f;
    }

    public void BackToOptions()
    {
        _optionsMenu.SetActive(true);
        _keyboardMenu.SetActive(false);
        _gamepadMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _brightness.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Keyboard()
    {
        _optionsMenu.SetActive(false);
        _keyboardMenu.SetActive(true);
        _brightness.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Gamepad()
    {
        _optionsMenu.SetActive(false);
        _gamepadMenu.SetActive(true);
        _brightness.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Settings()
    {
        _optionsMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        _brightness.SetActive(true);
        Time.timeScale = 0f;
    }
}
