using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SkillTreePointsView : MonoBehaviour
{
    [SerializeField] private PlayerExpSystem _playerExpSystem;
    [SerializeField] private Text _experienceText; // Referencia al texto en la UI

    void Update()
    {
        UpdateExperienceUI();
    }


    void UpdateExperienceUI()
    {
        if (_experienceText != null && _playerExpSystem != null)
        {
            _experienceText.text = "Experiencia disponible: " + _playerExpSystem._currentExp.ToString();
        }
    }
}
