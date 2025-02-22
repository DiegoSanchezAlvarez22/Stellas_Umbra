using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region Referencias
    [SerializeField] GameObject _menuWarning;
    [SerializeField] GameObject _menuMain;
    [SerializeField] GameObject _menuOptions;
    [SerializeField] GameObject _menuCredits;
    [SerializeField] GameObject _menuKeyboard;
    [SerializeField] GameObject _menuGamepad;
    [SerializeField] GameObject _menuSettings;

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
        _menuMain.SetActive(true);

        _menuWarning.SetActive(false);
        _menuOptions.SetActive(false);
        _menuCredits.SetActive(false);
        _menuGamepad.SetActive(false);
        _menuKeyboard.SetActive(false);
        _menuSettings.SetActive(false);
    }


    public void StartGame()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");

        // Guardamos que es un nuevo juego
        PlayerPrefs.SetInt("NewGame", 1);
        PlayerPrefs.SetInt("LoadGame", 0);
        PlayerPrefs.Save();

        // Cargar la escena
        SceneManager.LoadScene("DiseñoNivel1.1");
    }

    public void LoadGame()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");

        if (PlayerPrefs.HasKey("PlayerVida"))
        {
            // Guardamos que queremos cargar la partida
            PlayerPrefs.SetInt("NewGame", 0);
            PlayerPrefs.SetInt("LoadGame", 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene("DiseñoNivel1.1");
        }
        else 
        {
            StartCoroutine(WarningMesage());
        }
    }

    public void QuitGame()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");

        Application.Quit();
    }

    private IEnumerator WarningMesage()
    {
        _menuMain.SetActive(false);
        _menuWarning.SetActive (true);

        //Reproduce la animación
        _warningAnimator.Play("WarningAnimation");

        // Espera la duración de la animación
        yield return new WaitForSeconds(_warningAnimator.GetCurrentAnimatorStateInfo(0).length);

        _menuWarning.SetActive(false);
        _menuMain.SetActive(true);

    }
    public void BackToMainMenu()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");

        _menuOptions.SetActive(false);
        _menuCredits.SetActive(false);
        _menuMain.SetActive(true);
    }

    public void BackToOptionsMenu()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");

        _menuKeyboard.SetActive(false);
        _menuGamepad.SetActive(false);
        _menuSettings.SetActive(false);
        _menuOptions.SetActive(true);
    }

    public void OptionsMenu()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");

        _menuMain.SetActive(false);
        _menuOptions.SetActive(true);
    }

    public void CreditsMenu()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");

        _menuMain.SetActive(false);
        _menuCredits.SetActive(true);
    }

    public void KeyboardControlMenu()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");

        _menuOptions.SetActive(false);
        _menuKeyboard.SetActive(true);
    }

    public void GamepadControlMenu()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");

        _menuOptions.SetActive(false);
        _menuGamepad.SetActive(true);
    }

    public void ConfigMenu()
    {
        _audioManagerBehaviour.PlaySFX("ButtonClick");

        _menuOptions.SetActive(false);
        _menuSettings.SetActive(true);
    }
}
