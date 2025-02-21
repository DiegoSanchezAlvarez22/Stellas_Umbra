using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    AudioManagerBehaviour _audioManagerBehaviour;

    private void Awake()
    {
        _audioManagerBehaviour = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerBehaviour>();
    }

    public void GoMenu()
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

    public void GoGame()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
        SceneManager.LoadScene("DiseñoNivel1.1");
    }
    public void GoConfig()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
        SceneManager.LoadScene("Ajustes");
    }
    public void GoKeyboard()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
        SceneManager.LoadScene("ControlesTeclado");
    }
    public void GoGamepad()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
        SceneManager.LoadScene("ControlesMando");
    }
    public void Exit()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");
        Application.Quit();
    }

}
