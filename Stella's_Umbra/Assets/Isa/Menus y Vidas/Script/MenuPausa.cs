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
        _pInput.Enable();
    }
    public void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
        menuVidas.SetActive(true);
        _pInput.Enable();

    }

    public void PlaySound()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
    }
}
