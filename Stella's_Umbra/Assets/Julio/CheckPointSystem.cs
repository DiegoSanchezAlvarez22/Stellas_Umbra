using UnityEngine;

public class CheckPointSystem : MonoBehaviour
{
    #region Referencias
    //Referencia al Script de la experiencia
    [SerializeField] PlayerExpSystem _playerExpSystem;

    //Referencia al script de vida del jugador
    [SerializeField] VidaJugador _vidaJugador;

    //Referencia al Transform del jugador para guardar la posición
    [SerializeField] Transform _jugadorTransform;
    #endregion

    #region Claves
    //Clave para guardar/cargar el dato de la exp en PlayerPrefs
    const string ExpKey = "PlayerExp";

    //Clave para guardar/cargar el dato de la vida actual en PlayerPrefs
    const string VidaKey = "PlayerVida";

    //Clave para guardar/cargar el dato de la vida máxima en PlayerPrefs
    const string VidaMaxKey = "PlayerVidaMax";

    // Claves para guardar/cargar el dato de la posición en PlayerPrefs
    const string PosXKey = "PlayerPosX";
    const string PosYKey = "PlayerPosY";
    const string PosZKey = "PlayerPosZ";
    #endregion

    void Start()
    {
        // Se carga el progreso al iniciar el juego
        LoadProgress();
    }

    public void SaveProgress()
    {
        // Guardar la experiencia actual en PlayerPrefs
        if (_playerExpSystem != null)
        {
            PlayerPrefs.SetInt(ExpKey, _playerExpSystem.CurrentExp);
        }

        // Guardar la vida actual en PlayerPrefs
        if (_vidaJugador != null)
        {
            PlayerPrefs.SetInt(VidaKey, _vidaJugador.VidaActual);
            PlayerPrefs.SetInt(VidaMaxKey, _vidaJugador.VidaMaxima);
        }

        // Guardar la posición del CheckPoint en PlayerPrefs
        if (_jugadorTransform != null)
        {
            PlayerPrefs.SetFloat(PosXKey, _jugadorTransform.position.x);
            PlayerPrefs.SetFloat(PosYKey, _jugadorTransform.position.y);
            PlayerPrefs.SetFloat(PosZKey, _jugadorTransform.position.z);
        }

        // Guardar todo en PlayerPrefs
        PlayerPrefs.Save();
        Debug.Log("Progreso guardado.");
    }

    public void LoadProgress()
    {
        // Cargar la experiencia desde PlayerPrefs
        if (_playerExpSystem != null && PlayerPrefs.HasKey(ExpKey))
        {
            int savedExp = PlayerPrefs.GetInt(ExpKey);
            _playerExpSystem.SetCurrentExp(savedExp);
        }

        //Cargar la vida desde PlayerPrefs
        if (_vidaJugador != null)
        {
            // Cargar la vida actual desde PlayerPrefs
            if (PlayerPrefs.HasKey(VidaKey))
            {
                int savedVida = PlayerPrefs.GetInt(VidaKey);
                _vidaJugador.SetVidaActual(savedVida);
                _vidaJugador.cambioVida.Invoke(savedVida);
            }

            // Cargar la vida máxima desde PlayerPrefs
            if (PlayerPrefs.HasKey(VidaMaxKey))
            {
                int savedVidaMax = PlayerPrefs.GetInt(VidaMaxKey);
                _vidaJugador.SetVidaMaxima(savedVidaMax);
            }
        }

        // Cargar la posición desde PlayerPrefs
        if (_jugadorTransform != null)
        {
            if (PlayerPrefs.HasKey(PosXKey) && PlayerPrefs.HasKey(PosYKey) && PlayerPrefs.HasKey(PosZKey))
            {
                float posX = PlayerPrefs.GetFloat(PosXKey);
                float posY = PlayerPrefs.GetFloat(PosYKey);
                float posZ = PlayerPrefs.GetFloat(PosZKey);
                _jugadorTransform.position = new Vector3(posX, posY, posZ);

                Debug.Log("Posición cargada: " + _jugadorTransform.position);
            }
        }
        Debug.Log("Progreso cargado.");
    }

    public void ClearProgress()
    {
        // Para borrar el progreso guardado en PlayerPrefs
        PlayerPrefs.DeleteKey(ExpKey);
        PlayerPrefs.DeleteKey(VidaKey);
        PlayerPrefs.DeleteKey(VidaMaxKey);
        PlayerPrefs.DeleteKey(PosXKey);
        PlayerPrefs.DeleteKey(PosYKey);
        PlayerPrefs.DeleteKey(PosZKey);

        Debug.Log("Progreso eliminado.");
    }

    private void OnApplicationQuit()
    {
        ClearProgress();
        Debug.Log("Progreso borrado al cerrar el juego");
    }


    private void OnTriggereEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyAir"))
        {
            ClearProgress();

            Debug.Log("Progreso borrado al entrar en el teleport.");
        }
    }

}
