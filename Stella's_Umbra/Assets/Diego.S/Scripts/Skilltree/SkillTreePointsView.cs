using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SkillTreePointsView : MonoBehaviour
{
    [SerializeField] private PlayerExpSystem _playerExpSystem;
    [SerializeField] private Text _experienceText; // Referencia al texto en la UI
    private float _exp;

    void Start()
    {
        UpdateExperienceUI();
    }

    public void AddExperience(int amount)
    {
        _exp = _playerExpSystem._currentExp;
        UpdateExperienceUI();
    }

    void UpdateExperienceUI()
    {
        if (_experienceText != null)
        {
            _experienceText.text = "Experiencia disponible: " + _exp.ToString();
        }
    }
}
