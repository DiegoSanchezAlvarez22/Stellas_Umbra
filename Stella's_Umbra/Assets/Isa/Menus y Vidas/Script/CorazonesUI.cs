using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class CorazonesUI : MonoBehaviour
{
    public List<Image> listaCorazones;
    public Image prefabCorazonLleno;
    public Image prefabCorazonVacio;

    public VidaJugador vidaJugador;
    public int indexActual;
    public Sprite vidaLlena;
    public Sprite vidaVacia;

    private void Awake()
    {
        vidaJugador.cambioVida.AddListener(CambiarCorazones);
        vidaJugador.sumarCorazon.AddListener(SumarCorazones);
    }

    private void CambiarCorazones(int vidaActual)
    {
        if (!listaCorazones.Any())
        {
            CrearCorazones(vidaActual);
        }
        else
        {
            CambiarVida(vidaActual);
        }
    }

    private void CrearCorazones(int cantidadMaximaVida)
    {
        for (int i = 0; i < cantidadMaximaVida; i++)
        {
            Image corazon = Instantiate(prefabCorazonLleno, transform);
            listaCorazones.Add(corazon.GetComponent<Image>());
        }
        indexActual = cantidadMaximaVida - 1;
    }

    private void SumarCorazones(int sumar)
    {
        
        Image corazon = Instantiate(prefabCorazonVacio, transform);
        listaCorazones.Add(corazon.GetComponent<Image>());
        
        
    }

    private void CambiarVida(int vidaActual)
    {
        if (vidaActual <= indexActual)
        {
            PerderVida(vidaActual);
        }
        else
        {
            GanarVida(vidaActual);
        }
    }

    private void PerderVida(int vidaActual)
    {
        for (int i = indexActual; i >= vidaActual; i--)
        {
            indexActual= i;
            listaCorazones[i].GetComponent<Image>().sprite = vidaVacia;
            indexActual = i - 1;
        }
    
    }

    private void GanarVida(int vidaActual)
    {
        for (int i = indexActual; i < vidaActual; i++)
        {
            listaCorazones[i].GetComponent<Image>().sprite = vidaLlena;
            indexActual = i;
        }
    }
}
