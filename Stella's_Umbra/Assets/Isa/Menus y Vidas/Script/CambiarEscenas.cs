using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenas : MonoBehaviour
{
    AudioManagerBehaviour _audioManagerBehaviour;

    private void Awake()
    {
        _audioManagerBehaviour = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerBehaviour>();
    }

    public void IrMenu()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu Principal");
    }
    /*public void IrAOpciones()
    {
        SceneManager.LoadScene("Opciones");
    }
    public void IrACreditos()
    {
        SceneManager.LoadScene("Creditos");
    }*/

    public void IrJuego()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
        SceneManager.LoadScene("DiseñoNivel1.1");
    }
    public void IrAjustes()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
        SceneManager.LoadScene("Ajustes");
    }
    public void IrTeclado()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
        SceneManager.LoadScene("ControlesTeclado");
    }
    public void IrMando()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
        SceneManager.LoadScene("ControlesMando");
    }
    public void Salir()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
        Application.Quit();
    }

}
