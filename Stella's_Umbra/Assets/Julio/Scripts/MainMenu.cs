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

    [SerializeField] Animator _warningAnimator;

    CheckPointSystem _checkPointSystem;

    AudioManagerBehaviour _audioManagerBehaviour;
    #endregion

    private void Awake()
    {
        _audioManagerBehaviour = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerBehaviour>();
    }

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
        _audioManagerBehaviour.PlaySFX(_audioManagerBehaviour.buttonClick);

        // Guardamos que es un nuevo juego
        PlayerPrefs.SetInt("NewGame", 1);
        PlayerPrefs.SetInt("LoadGame", 0);
        PlayerPrefs.Save();

        // Cargar la escena
        SceneManager.LoadScene("DiseñoNivel1.1");
    }

    public void CargarPartida()
    {
        _audioManagerBehaviour.PlaySFX(_audioManagerBehaviour.buttonClick);

        if (PlayerPrefs.HasKey("PlayerVida"))
        {
            PlayerPrefs.SetInt("NewGame", 0);
            // Guardamos que queremos cargar la partida
            PlayerPrefs.SetInt("LoadGame", 1);
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
        _audioManagerBehaviour.PlaySFX(_audioManagerBehaviour.buttonClick);

        Application.Quit();
    }

    private IEnumerator MostrarMensajeWarning()
    {
        _menuPrincipal.SetActive(false);
        _menuWarning.SetActive (true);

        //Reproduce la animación
        _warningAnimator.Play("WarningAnimation");

        // Espera la duración de la animación
        yield return new WaitForSeconds(_warningAnimator.GetCurrentAnimatorStateInfo(0).length);

        _menuWarning.SetActive(false);
        _menuPrincipal.SetActive(true);

    }
    public void VolverMenuPrincipal()
    {
        _audioManagerBehaviour.PlaySFX(_audioManagerBehaviour.buttonClick);

        _menuOpciones.SetActive(false);
        _menuCreditos.SetActive(false);
        _menuPrincipal.SetActive(true);
    }

    public void VolverMenuOpciones()
    {
        _audioManagerBehaviour.PlaySFX(_audioManagerBehaviour.buttonClick);

        _menuControlTeclado.SetActive(false);
        _menuControlMando.SetActive(false);
        _menuAjustes.SetActive(false);
        _menuOpciones.SetActive(true);
    }

    public void MenuOpciones()
    {
        _audioManagerBehaviour.PlaySFX(_audioManagerBehaviour.buttonClick);

        _menuPrincipal.SetActive(false);
        _menuOpciones.SetActive(true);
    }

    public void MenuCreditos()
    {
        _audioManagerBehaviour.PlaySFX(_audioManagerBehaviour.buttonClick);

        _menuPrincipal.SetActive(false);
        _menuCreditos.SetActive(true);
    }

    public void MenuControlTeclado()
    {
        _audioManagerBehaviour.PlaySFX(_audioManagerBehaviour.buttonClick);

        _menuOpciones.SetActive(false);
        _menuControlTeclado.SetActive(true);
    }

    public void MenuControlMando()
    {
        _audioManagerBehaviour.PlaySFX(_audioManagerBehaviour.buttonClick);

        _menuOpciones.SetActive(false);
        _menuControlMando.SetActive(true);
    }

    public void MenuAjustes()
    {
        _audioManagerBehaviour.PlaySFX(_audioManagerBehaviour.buttonClick);

        _menuOpciones.SetActive(false);
        _menuAjustes.SetActive(true);
    }
}
