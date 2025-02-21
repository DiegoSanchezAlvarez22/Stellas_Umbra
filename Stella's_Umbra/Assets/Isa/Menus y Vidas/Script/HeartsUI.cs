using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public List<Image> _heartsList;
    public Image _fullHeartPrefab;
    public Image _emptyHeartPrefab;

    public PlayerLife _playerLifes;
    public int _actualIndex;
    public Sprite _fullLife;
    public Sprite _emptyLife;

    private void Awake()
    {
        _playerLifes._changeLife.AddListener(ChangeHearts);
        _playerLifes._increaseHeart.AddListener(MoreHearts);
    }

    public void ChangeHearts(int vidaActual)
    {
        //JULIO Si no hay corazones en la lista, los creamos
        if (!_heartsList.Any())
        {
            CreateHearts(vidaActual);
        }
        else
        {
            //Actualizar corazones existentes con la vida actual
            ActualizeHearts(vidaActual);
        }
    }

    private void CreateHearts(int cantidadMaximaVida)
    {
        for (int i = 0; i < cantidadMaximaVida; i++)
        {
            Image corazon = Instantiate(_fullHeartPrefab, transform);
            _heartsList.Add(corazon.GetComponent<Image>());
        }
    }

    //JULIO
    private void ActualizeHearts(int vidaActual)
    {
        //Asegúrate que la lista tiene suficientes elem para la vida actual
        for (int i = 0; i < _heartsList.Count; i++)
        {
            if (i < vidaActual)
            {
                //Corazón lleno
                _heartsList[i].sprite = _fullLife;
            }
            else
            {
                //Corazón vacío 
                _heartsList[i].sprite = _emptyLife;
            }
        }
    }

    public void MoreHearts(int sumar)
    {
        Image corazon = Instantiate(_emptyHeartPrefab, transform);
        _heartsList.Add(corazon.GetComponent<Image>());
    }

    private void ChangeLife(int vidaActual)
    {
        if (vidaActual <= _actualIndex)
        {
            LooseLife(vidaActual);
        }
        else
        {
            GetLife(vidaActual);
        }
    }

    private void LooseLife(int vidaActual)
    {
        for (int i = _actualIndex; i >= vidaActual; i--)
        {
            _actualIndex= i;
            _heartsList[i].GetComponent<Image>().sprite = _emptyLife;
            _actualIndex = i - 1;
        }
    
    }

    private void GetLife(int vidaActual)
    {
        for (int i = _actualIndex; i < vidaActual; i++)
        {
            _heartsList[i].GetComponent<Image>().sprite = _fullLife;
            _actualIndex = i;
        }
    }
}
