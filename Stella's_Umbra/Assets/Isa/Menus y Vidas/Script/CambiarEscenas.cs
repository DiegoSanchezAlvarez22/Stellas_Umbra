using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenas : MonoBehaviour
{
    public void IrMenu()
    {
        SceneManager.LoadScene("Menu Principal");
    }
    public void IrAOpciones()
    {
        SceneManager.LoadScene("Opciones");
    }
    public void IrACreditos()
    {
        SceneManager.LoadScene("Creditos");
    }
    public void IrJuego()
    {
        SceneManager.LoadScene("Game");
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
