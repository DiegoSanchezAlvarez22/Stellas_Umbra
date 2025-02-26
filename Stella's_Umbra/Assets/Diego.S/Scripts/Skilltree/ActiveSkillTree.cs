using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveSkillTree : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _skilltreeAction;
    private PlayerMov _playerMov;

    [SerializeField] private GameObject _skilltree;
    [SerializeField] private GameObject _starsBackground;
    [SerializeField] private GameObject _pointer;

    [SerializeField] GameObject _lifeCanvas;
    [SerializeField] GameObject _mapIcon;
    [SerializeField] MapBehaviour _mapaDesplegable;
    [SerializeField] private GameObject _bossHealthBar;

    bool mapWasOpen = false;

    private bool juegoPausado = false;

    bool bossHealthBarWasActive = false;

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
    #endregion

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _skilltreeAction = _playerInput.actions["SkillTreeAction"];

        _playerMov = GetComponent<PlayerMov>();

        _walk = _playerInput.actions["Walk"];
        _bendDown = _playerInput.actions["BendDown"];
        _jump = _playerInput.actions["Jump"];
        _superJump = _playerInput.actions["SuperJump"];
        _dash = _playerInput.actions["Dash"];
        _moveObj = _playerInput.actions["MoveObj"];
        _interact = _playerInput.actions["Interact"];
        _basicAttack = _playerInput.actions["BasicAttack"];
        _boulderAttack = _playerInput.actions["BoulderAttack"];
        _energyOrbAttack = _playerInput.actions["EnergyOrbAttack"];
        _tornadoAttack = _playerInput.actions["TornadoAttack"];
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
        _skilltreeAction.performed += ToggleSkillTree;
    }

    private void OnDisable()
    {
        _skilltreeAction.performed -= ToggleSkillTree;
    }

    private void Start()
    {
        _skilltree.SetActive(false);
        _pointer.SetActive(false);
    }

    private void ToggleSkillTree(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (juegoPausado)
            {
                DeactivateSkillTree();
                Enable();

                //Reaparece la barra de vida si estaba activa antes de pausar el juego
                if (bossHealthBarWasActive)
                {
                    _bossHealthBar.SetActive(true);
                }
            }
            else
            {
                ActivateSkillTree();
                DisableSkills();

                //Guarda si estaba activa la barra de vida del Boss antes de pausar
                bossHealthBarWasActive = _bossHealthBar.activeSelf;
                _bossHealthBar.SetActive(false);
            }
        }
    }

    public void ActivateSkillTree()
    {
        if (_skilltree != null)
        {

            juegoPausado = true;
            Time.timeScale = 0f;
            _skilltree.SetActive(true);
            _pointer.SetActive(true);
            _starsBackground.SetActive(true);

            _lifeCanvas.SetActive(false);
            _mapIcon.SetActive(false);

            //Guarda si el mapa estaba abierto antes de pausar
            mapWasOpen = _mapaDesplegable._isOpen;

            if (mapWasOpen)
            {
                _mapaDesplegable._anim.Play("MapaCerrado");
            }
        }
        else
        {
            Debug.Log("No se puede usar el skilltree");
        }
    }

    public void DeactivateSkillTree()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        _skilltree.SetActive(false);
        _pointer.SetActive(false);
        _starsBackground.SetActive(false);

        _lifeCanvas.SetActive(true);
        _mapIcon.SetActive(true);

        //Si el mapa estaba abierto antes de pausar, lo volvemos a abrir
        if (mapWasOpen)
        {
            _mapaDesplegable._anim.Play("MapaAbierto");
        }
    }
}