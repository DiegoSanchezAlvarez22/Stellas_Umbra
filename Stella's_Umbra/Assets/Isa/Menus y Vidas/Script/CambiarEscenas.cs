using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenas : MonoBehaviour
{
    public void IrMenu()
    {
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
        SceneManager.LoadScene("DiseñoNivel1.1");
    }
    public void IrAjustes()
    {
        SceneManager.LoadScene("Ajustes");
    }
    public void IrTeclado()
    {
        SceneManager.LoadScene("ControlesTeclado");
    }
    public void IrMando()
    {
        SceneManager.LoadScene("ControlesMando");
    }
    public void Salir()
    {
        Application.Quit();
    }

}
