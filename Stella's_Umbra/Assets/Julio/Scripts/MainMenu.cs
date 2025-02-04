using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region Referencias
    [SerializeField] GameObject _menuWarning;
    [SerializeField] GameObject _menuPrincipal;
    [SerializeField] GameObject _menuOpciones;
    [SerializeField] GameObject _menuCreditos;
    [SerializeField] GameObject _menuControlTeclado;
    [SerializeField] GameObject _menuControlMando;
    [SerializeField] GameObject _menuAjustes;

    CheckPointSystem _checkPointSystem;
    #endregion

    void Start()
    {
        _menuPrincipal.SetActive(true);

        _menuWarning.SetActive(false);
        _menuOpciones.SetActive(false);
        _menuCreditos.SetActive(false);
        _menuControlMando.SetActive(false);
        _menuControlTeclado.SetActive(false);
        _menuAjustes.SetActive(false);
    }


    public void IniciarJuego()
    {
        PlayerPrefs.SetInt("NewGame", 1); // Guardamos que es un nuevo juego
        PlayerPrefs.SetInt("LoadGame", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene("DiseñoNivel1.1"); // Cargar la escena
    }

    public void CargarPartida()
    {
        if (PlayerPrefs.HasKey("PlayerVida"))
        {
            PlayerPrefs.SetInt("NewGame", 0);
            PlayerPrefs.SetInt("LoadGame", 1); // Guardamos que queremos cargar la partida
            PlayerPrefs.Save();

            SceneManager.LoadScene("DiseñoNivel1.1");
        }
        else 
        {
            StartCoroutine(MostrarMensajeWarning());
        }
    }

    public void SalirJuego()
    {
        Application.Quit();
    }

    private IEnumerator MostrarMensajeWarning()
    {
        _menuPrincipal.SetActive(false);
        _menuWarning.SetActive (true);
        yield return new WaitForSeconds(2);
        _menuWarning.SetActive(false);
        _menuPrincipal.SetActive(true);

    }
    public void VolverMenuPrincipal()
    {
        _menuOpciones.SetActive(false);
        _menuCreditos.SetActive(false);
        _menuPrincipal.SetActive(true);
    }

    public void VolverMenuOpciones()
    {
        _menuControlTeclado.SetActive(false);
        _menuControlMando.SetActive(false);
        _menuAjustes.SetActive(false);
        _menuOpciones.SetActive(true);
    }

    public void MenuOpciones()
    {
        _menuPrincipal.SetActive(false);
        _menuOpciones.SetActive(true);
    }

    public void MenuCreditos()
    {
        _menuPrincipal.SetActive(false);
        _menuCreditos.SetActive(true);
    }

    public void MenuControlTeclado()
    {
        _menuOpciones.SetActive(false);
        _menuControlTeclado.SetActive(true);
    }

    public void MenuControlMando()
    {
        _menuOpciones.SetActive(false);
        _menuControlMando.SetActive(true);
    }

    public void MenuAjustes()
    {
        _menuOpciones.SetActive(false);
        _menuAjustes.SetActive(true);
    }
}
