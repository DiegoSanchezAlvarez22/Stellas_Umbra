using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpSystem : MonoBehaviour
{
    public static PlayerExpSystem _playerExpSystemInstance;
    public int _currentExp = 100;

    //JULIO Propiedad para obtener la experiencia actual
    public int CurrentExp => _currentExp;
    void Awake()
    {
        _playerExpSystemInstance = this;
    }

    void Start()
    {
        UpdateExp();
    }

    public void UpdateExp()
    {
        //_expText.SetText("Puntos: "+ _currentExp);
    }

    public void AddExp(int _newExp)
    {
        _currentExp += _newExp;
        Debug.Log("Exp aumentada a: " + _currentExp);
    }

    public void SubtractExp(int _expLost)
    {
        _currentExp -= _expLost;
        Debug.Log("Exp diasminuida a: " + _currentExp);
    }

    //JULIO Propiedad para obtener experiencia actual
    public void SetCurrentExp(int newExp)
    {
        _currentExp = newExp;
        Debug.Log("Experiencia establecida a: " + _currentExp);
    }
}
