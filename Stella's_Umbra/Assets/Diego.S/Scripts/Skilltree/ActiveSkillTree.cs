using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveSkillTree : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _skilltreeAction;

    [SerializeField] private GameObject _skilltree;
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
        juegoPausado = true;
        Time.timeScale = 0f;
        _skilltree.SetActive(true);
    }

    public void DeactivateSkillTree()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        _skilltree.SetActive(false);
    }
}