using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject menuVidas;

    private bool juegoPausado = false;
    private void Start()
    {
        menuPausa.SetActive(false);
        menuVidas.SetActive(true);

        Time.timeScale = 1f;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        { 
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausa();
            }
        }
    }

    public void Pausa()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        menuPausa.SetActive(true);
        menuVidas.SetActive(false);
    }
    public void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
        menuVidas.SetActive(true);
    }
}
