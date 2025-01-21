using System.Runtime.CompilerServices;
using UnityEditor.Build.Content;
using UnityEngine;

public class ActiveSkillTree : MonoBehaviour
{
    [SerializeField] private GameObject _skilltree;
    private bool juegoPausado = false;

    private void Start()
    {
        _skilltree.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
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
