using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveSkillTree : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _skilltreeAction;

    [SerializeField] private GameObject _skilltree;
    [SerializeField] private GameObject _starsBackground;

    [SerializeField] GameObject _lifeCanvas;
    [SerializeField] GameObject _mapIcon;
    [SerializeField] MapaDesplegable _mapaDesplegable;

    bool mapWasOpen = false;

    private bool juegoPausado = false;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _skilltreeAction = _playerInput.actions["SkillTreeAction"];
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
    }

    private void ToggleSkillTree(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (juegoPausado)
            {
                DeactivateSkillTree();
            }
            else
            {
                ActivateSkillTree();
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
            _starsBackground.SetActive(true);

            _lifeCanvas.SetActive(false);
            _mapIcon.SetActive(false);
            //Guarda si el mapa estaba abierto antes de pausar
            mapWasOpen = _mapaDesplegable.estaAbierto;
            if (mapWasOpen)
            {
                //Cierra el mapa
                _mapaDesplegable.mapaAnimator.Play("MapaCerrado");
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
        _starsBackground.SetActive(false);

        _lifeCanvas.SetActive(true);
        _mapIcon.SetActive(true);
        //Si el mapa estaba abierto antes de pausar, lo volvemos a abrir
        if (mapWasOpen)
        {
            _mapaDesplegable.mapaAnimator.Play("MapaAbierto");
        }
    }
}