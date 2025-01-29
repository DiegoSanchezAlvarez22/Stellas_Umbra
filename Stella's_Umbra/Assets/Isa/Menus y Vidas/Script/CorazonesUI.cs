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

    public void CambiarCorazones(int vidaActual)
    {
        //JULIO Si no hay corazones en la lista, los creamos
        if (!listaCorazones.Any())
        {
            CrearCorazones(vidaActual);
        }
        else
        {
            //Actualizar corazones existentes con la vida actual
            ActualizarCorazones(vidaActual);
        }
    }

    private void CrearCorazones(int cantidadMaximaVida)
    {
        for (int i = 0; i < cantidadMaximaVida; i++)
        {
            Image corazon = Instantiate(prefabCorazonLleno, transform);
            listaCorazones.Add(corazon.GetComponent<Image>());
        }
    }

    //JULIO
    private void ActualizarCorazones(int vidaActual)
    {
        //Asegúrate que la lista tiene suficientes elem para la vida actual
        for (int i = 0; i < listaCorazones.Count; i++)
        {
            if (i < vidaActual)
            {
                //Corazón lleno
                listaCorazones[i].sprite = vidaLlena;
            }
            else
            {
                //Corazón vacío 
                listaCorazones[i].sprite = vidaVacia;
            }
        }
    }

    public void SumarCorazones(int sumar)
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
