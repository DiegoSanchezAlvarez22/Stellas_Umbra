using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] GameObject menuPausa;
    [SerializeField] GameObject menuVidas;
    [SerializeField] GameObject fondoPausa;
    [SerializeField] GameObject menuOpciones;
    [SerializeField] GameObject menuTeclado;
    [SerializeField] GameObject menuMando;
    [SerializeField] GameObject menuAjustes;
    [SerializeField] GameObject brillo;


    private bool juegoPausado = false;

    [SerializeField] private InputActionAsset _pInput;

    AudioManagerBehaviour _audioManagerBehaviour;

    public void EnableControls()
    {
        _pInput.Enable();
    }

    public void DisableControls()
    {
        _pInput.Disable();
    }

    private void Awake()
    {
        _audioManagerBehaviour = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerBehaviour>();
    }

    private void Start()
    {
        menuPausa.SetActive(false);
        menuVidas.SetActive(true);
        fondoPausa.SetActive(false);
        menuOpciones.SetActive(false);
        menuTeclado.SetActive(false);
        menuMando.SetActive(false);
        menuAjustes.SetActive(false);
        brillo.SetActive(false);
        _pInput.Enable();

        Time.timeScale = 1f;
    }
    private void Update()
    {
        if (UnityEngine.Input.GetKeyUp(KeyCode.Escape)) 
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
        fondoPausa.SetActive(true);
        menuOpciones.SetActive(false);
        menuTeclado.SetActive(false);
        menuMando.SetActive(false);
        menuAjustes.SetActive(false);
        brillo.SetActive(true);
        _pInput.Enable(); 
    }
    public void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
        menuVidas.SetActive(true);
        fondoPausa.SetActive(false);
        menuOpciones.SetActive(false);
        menuTeclado.SetActive(false);
        menuMando.SetActive(false);
        menuAjustes.SetActive(false);
        brillo.SetActive(false);
        _pInput.Enable();

    }

    public void PlaySound()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
    }

    public void Opciones()
    {
        menuPausa.SetActive(false);
        menuOpciones.SetActive(true);
        brillo.SetActive(true);
        Time.timeScale = 0f;
        _pInput.Enable();
    }

    public void VolverPausa()
    {
        menuPausa.SetActive(true);
        menuOpciones.SetActive(false);
        brillo.SetActive(true);
        Time.timeScale = 0f;
        _pInput.Enable();
    }

    public void VolverOpciones()
    {
        menuOpciones.SetActive(true);
        menuTeclado.SetActive(false);
        menuMando.SetActive(false);
        menuAjustes.SetActive(false);
        brillo.SetActive(true);
        Time.timeScale = 0f;
        _pInput.Enable();
    }

    public void Teclado()
    {
        menuOpciones.SetActive(false);
        menuTeclado.SetActive(true);
        brillo.SetActive(true);
        Time.timeScale = 0f;
        _pInput.Enable();
    }

    public void Mando()
    {
        menuOpciones.SetActive(false);
        menuMando.SetActive(true);
        brillo.SetActive(true);
        Time.timeScale = 0f;
        _pInput.Enable();
    }

    public void Ajustes()
    {
        menuOpciones.SetActive(false);
        menuAjustes.SetActive(true);
        brillo.SetActive(true);
        Time.timeScale = 0f;
        _pInput.Enable();
    }
}
