using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointSystem : MonoBehaviour
{
    #region References
    //Referencia al Transform del jugador para guardar la posición
    [SerializeField] Transform _playerTransform;

    //Referencia al script del PlayerMov
    [SerializeField] PlayerMov _playerMov;

    //Referencia al script PlayerAttacks
    [SerializeField] PlayerAttacks _playerAttacks;

    //Referencia al script de vida del jugador
    [SerializeField] PlayerLife _playerLifes;

    //Referencia al Script de la experiencia
    [SerializeField] PlayerExpSystem _playerExpSystem;

    //Referencia al script del Skill Tree_Manager
    private SkillTree_Manager _skillTreeManager;

    //Referencia al Transform del canvas de Corazones
    [SerializeField] Transform _heartsCanvasTransform;
    #endregion

    #region Keys
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

    //Claves para guardar/cargar el dato de los cristales en PlayerPrefs
    const string CristalesKey = "PlayerCristales";

    //Claves para guardar/cargar las habilidades PlayerMov en PlayerPrefs
    const string CanJumpKey = "PlayerCanJump";
    const string CanSuperJumpKey = "PlayerCanSuperJump";
    const string CanDashKey = "PlayerCanDash";
    const string CanWallJumpKey = "PlayerCanWallJump";
    const string CanMoveObjKey = "PlayerCanMoveObj";

    const string JumpsLeftKey = "PlayerJumpsLeft";
    const string JumpsLeftMaxKey = "PlayerJumpsLeftMax";

    //Claves para guardar/cargar los puntos del árbol de habilidades en PlayerPrefs
    const string SkillPointsKey = "PlayerSkillPoints";

    //Claves para guardar/cargar los datos PlayerAttack en PlayerPrefs
    const string EnergyKey = "PlayerEnergy";
    const string EnergyBoostKey = "PlayerEnergyBoost";
    const string CanBasicAttackKey = "PlayerCanBasicAttack";
    const string CanBoulderAttackKey = "PlayerCanBoulderAttack";
    const string CanTornadoAttackKey = "PlayerCanTornadoAttack";
    const string CanEnergyOrbAttackKey = "PlayerCanEnergyOrbAttack";

    //Clave para guardar/cargar la posición del canvas de corazones
    const string HeartPosXKey = "HeartCanvasPosX";
    const string HeartPosYKey = "HeartCanvasPosY";
    const string HeartPosZKey = "HeartCanvasPosZ";
    #endregion

    void Start()
    {
        // Se carga el progreso al iniciar el juego
        //LoadProgress();
    }

    public void SaveProgress()
    {
        // Guardar la experiencia actual en PlayerPrefs
        if (_playerExpSystem != null)
        {
            PlayerPrefs.SetInt(ExpKey, _playerExpSystem.CurrentExp);
            Debug.Log("Experiencia guardada: " + _playerExpSystem.CurrentExp);
        }

        // Guardar la vida actual en PlayerPrefs
        if (_playerLifes != null)
        {
            PlayerPrefs.SetInt(VidaKey, _playerLifes.ActualLife);
            PlayerPrefs.SetInt(VidaMaxKey, _playerLifes.MaxLife);
            PlayerPrefs.SetInt(CristalesKey, _playerLifes.ActualCrystalsTaken);
            Debug.Log("Vida actual guardada: " + _playerLifes.ActualLife);
            Debug.Log("Vida máxima guardada: " + _playerLifes.MaxLife);
            Debug.Log("Cristales guardados: " + _playerLifes.ActualCrystalsTaken);
        }

        // Guardar la posición del CheckPoint en PlayerPrefs
        if (_playerTransform != null)
        {
            PlayerPrefs.SetFloat(PosXKey, _playerTransform.position.x);
            PlayerPrefs.SetFloat(PosYKey, _playerTransform.position.y);
            PlayerPrefs.SetFloat(PosZKey, _playerTransform.position.z);
            Debug.Log("Posición guardada: " + _playerTransform.position.x + _playerTransform.position.y + _playerTransform.position.z);
        }

        //Guardar la información del PlayerMov en PlayerPrefs
        if (_playerMov != null)
        {
            PlayerPrefs.SetInt(CanJumpKey, _playerMov._canJump ? 1 : 0);
            PlayerPrefs.SetInt(CanSuperJumpKey, _playerMov._canSuperJump ? 1 : 0);
            PlayerPrefs.SetInt(CanDashKey, _playerMov._canDash ? 1 : 0);
            PlayerPrefs.SetInt(CanWallJumpKey, _playerMov._canWallJump ? 1 : 0);
            PlayerPrefs.SetInt(CanMoveObjKey, _playerMov._canMoveObj ? 1 : 0);
            Debug.Log("Salto guardado: " + (_playerMov._canJump ? 1 : 0));
            Debug.Log("Super Salto guardado: " + (_playerMov._canSuperJump ? 1 : 0));
            Debug.Log("Dash guardado: " + (_playerMov._canDash ? 1 : 0));
            Debug.Log("Wall Jump guardado: " + (_playerMov._canWallJump ? 1 : 0));
            Debug.Log("Mover objeto guardado: " + (_playerMov._canMoveObj ? 1 : 0));

            PlayerPrefs.SetFloat(JumpsLeftKey, _playerMov._jumpsLeft);
            PlayerPrefs.SetInt(JumpsLeftMaxKey, _playerMov._jumpsLeftMax);
            Debug.Log("Saltos restantes guardados: " + (_playerMov._jumpsLeft));
            Debug.Log("Saltos máximos restantes guardados: " + (_playerMov._jumpsLeftMax));
        }

        // Guardar Skill Points del Skill Tree_Manager en PlayerPrefs
        if (_skillTreeManager != null)
        {
            PlayerPrefs.SetInt(SkillPointsKey, _skillTreeManager.skillPoints);
            Debug.Log("Skill Points guardados: " + _skillTreeManager.skillPoints);
        }

        // Guardar información de PlayerAttacks en PlayerPrefs
        if (_playerAttacks != null)
        {
            PlayerPrefs.SetFloat(EnergyKey, _playerAttacks._energy);
            PlayerPrefs.SetFloat(EnergyBoostKey, _playerAttacks._energyBoost);
            PlayerPrefs.SetInt(CanBasicAttackKey, _playerAttacks._canBasicAttack ? 1 : 0);
            PlayerPrefs.SetInt(CanBoulderAttackKey, _playerAttacks._canBoulderAttack ? 1 : 0);
            PlayerPrefs.SetInt(CanTornadoAttackKey, _playerAttacks._canTornadoAttack ? 1 : 0);
            PlayerPrefs.SetInt(CanEnergyOrbAttackKey, _playerAttacks._canEnergyOrbAttack ? 1 : 0);

            Debug.Log("Energía del jugador: " + _playerAttacks._energy);
            Debug.Log("Boost de energía: " + _playerAttacks._energyBoost);
            Debug.Log("Puede hacer ataque básico: " + (_playerAttacks._canBasicAttack ? 1 : 0));
            Debug.Log("Puede hacer ataque boulder: " + (_playerAttacks._canBoulderAttack ? 1 : 0));
            Debug.Log("Puede hacer ataque tornado: " + (_playerAttacks._canTornadoAttack ? 1 : 0));
            Debug.Log("Puede hacer ataque fijado: " + (_playerAttacks._canEnergyOrbAttack ? 1 : 0));
        }

        if (_heartsCanvasTransform != null)
        {
            PlayerPrefs.SetFloat(HeartPosXKey, _heartsCanvasTransform.position.x);
            PlayerPrefs.SetFloat(HeartPosYKey, _heartsCanvasTransform.position.y);
            PlayerPrefs.SetFloat(HeartPosZKey, _heartsCanvasTransform.position.z);
        }

        // Guardar todo en PlayerPrefs
        PlayerPrefs.Save();
        Debug.Log("Progreso guardado.");
    }

    public void LoadProgress()
    {

        //Si mueres sin pasar por checkpoints te lleva a la escena de menu principal
        if (_playerLifes != null && _playerLifes.ActualLife == 0)
        {
            if (!PlayerPrefs.HasKey(VidaKey))
            {
                SceneManager.LoadScene("Menu Principal");
                return;
            }
        }


        // Cargar la experiencia desde PlayerPrefs
        if (_playerExpSystem != null && PlayerPrefs.HasKey(ExpKey))
        {
            int savedExp = PlayerPrefs.GetInt(ExpKey);
            _playerExpSystem.SetCurrentExp(savedExp);
        }

        //Cargar la vida desde PlayerPrefs
        if (_playerLifes != null)
        {
            // Cargar la vida máxima desde PlayerPrefs
            if (PlayerPrefs.HasKey(VidaMaxKey))
            {
                int savedVidaMax = PlayerPrefs.GetInt(VidaMaxKey);
                _playerLifes.SetMaxLife(savedVidaMax);
            }

            // Cargar la vida actual desde PlayerPrefs
            if (PlayerPrefs.HasKey(VidaKey))
            {
                int savedVida = PlayerPrefs.GetInt(VidaKey);
                _playerLifes.SetActualLife(savedVida);
            }

            //Cargar los cristales desde PlayerPrefs
            if (PlayerPrefs.HasKey(CristalesKey))
            {
                int savedCristales = PlayerPrefs.GetInt(CristalesKey);
                _playerLifes.SetCrystalsNumber(savedCristales);
            }
        }

        // Cargar la posición desde PlayerPrefs
        if (_playerTransform != null)
        {
            if (PlayerPrefs.HasKey(PosXKey) && PlayerPrefs.HasKey(PosYKey) && PlayerPrefs.HasKey(PosZKey))
            {
                float posX = PlayerPrefs.GetFloat(PosXKey);
                float posY = PlayerPrefs.GetFloat(PosYKey);
                float posZ = PlayerPrefs.GetFloat(PosZKey);
                _playerTransform.position = new Vector3(posX, posY, posZ);

                _playerLifes._deathCanvas.SetActive(false);
                _playerLifes._starsBackground.SetActive(false);
            }
            else
            {
                //SceneManager.LoadScene("Manu Principal");
            }
        }

        //Cargar la info de PlayerMov desde PlayerPrefs
        if (_playerMov != null)
        {
            _playerMov._canJump = PlayerPrefs.GetInt(CanJumpKey, 0) == 1;
            _playerMov._canSuperJump = PlayerPrefs.GetInt(CanSuperJumpKey, 0) == 1;
            _playerMov._canDash = PlayerPrefs.GetInt(CanDashKey, 0) == 1;
            _playerMov._canWallJump = PlayerPrefs.GetInt(CanWallJumpKey, 0) == 1;
            _playerMov._canMoveObj = PlayerPrefs.GetInt(CanMoveObjKey, 0) == 1;

            if (PlayerPrefs.HasKey(JumpsLeftKey))
            {
                _playerMov._jumpsLeft = PlayerPrefs.GetFloat(JumpsLeftKey);
            }
            if (PlayerPrefs.HasKey(JumpsLeftMaxKey))
            {
                _playerMov._jumpsLeftMax = PlayerPrefs.GetInt(JumpsLeftMaxKey);
            }
        }

        // Cargar Skill Points del Skill Tree_Manager desde PlayerPrefs
        if (_skillTreeManager != null && PlayerPrefs.HasKey(SkillPointsKey))
        {
            _skillTreeManager.skillPoints = PlayerPrefs.GetInt(SkillPointsKey);
            _skillTreeManager.UpdateSkillPoints();
            Debug.Log("Skill Points cargados: " + _skillTreeManager.skillPoints);
        }

        // Cargar información de PlayerAttacks desde PlayerPrefs
        if (_playerAttacks != null)
        {
            _playerAttacks._canBasicAttack = PlayerPrefs.GetInt(CanBasicAttackKey, 0) == 1;
            _playerAttacks._canBoulderAttack = PlayerPrefs.GetInt(CanBoulderAttackKey, 0) == 1;
            _playerAttacks._canTornadoAttack = PlayerPrefs.GetInt(CanTornadoAttackKey, 0) == 1;
            _playerAttacks._canEnergyOrbAttack = PlayerPrefs.GetInt(CanEnergyOrbAttackKey, 0) == 1;

            if (PlayerPrefs.HasKey(EnergyKey))
            {
                _playerAttacks._energy = PlayerPrefs.GetFloat(EnergyKey);
            }
            if (PlayerPrefs.HasKey(EnergyBoostKey))
            {
                _playerAttacks._energyBoost = PlayerPrefs.GetFloat(EnergyBoostKey);
            }
        }

        if (_heartsCanvasTransform != null)
        {
            if (PlayerPrefs.HasKey(HeartPosXKey) && PlayerPrefs.HasKey(HeartPosYKey) && PlayerPrefs.HasKey(HeartPosZKey))
            {
                float heartPosX = PlayerPrefs.GetFloat(HeartPosXKey);
                float heartPosY = PlayerPrefs.GetFloat(HeartPosYKey);
                float heartPosZ = PlayerPrefs.GetFloat(HeartPosZKey);
                _heartsCanvasTransform.position = new Vector3(heartPosX, heartPosY, heartPosZ);
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
        PlayerPrefs.DeleteKey(CristalesKey);
        PlayerPrefs.DeleteKey(CanJumpKey);
        PlayerPrefs.DeleteKey(CanSuperJumpKey);
        PlayerPrefs.DeleteKey(CanDashKey);
        PlayerPrefs.DeleteKey(CanWallJumpKey);
        PlayerPrefs.DeleteKey(CanMoveObjKey);
        PlayerPrefs.DeleteKey(JumpsLeftKey);
        PlayerPrefs.DeleteKey(JumpsLeftMaxKey);
        PlayerPrefs.DeleteKey(SkillPointsKey);
        PlayerPrefs.DeleteKey(EnergyKey);
        PlayerPrefs.DeleteKey(EnergyBoostKey);
        PlayerPrefs.DeleteKey(CanBasicAttackKey);
        PlayerPrefs.DeleteKey(CanBoulderAttackKey);
        PlayerPrefs.DeleteKey(CanTornadoAttackKey);
        PlayerPrefs.DeleteKey(CanEnergyOrbAttackKey);
        PlayerPrefs.DeleteKey(HeartPosXKey);
        PlayerPrefs.DeleteKey(HeartPosYKey);
        PlayerPrefs.DeleteKey(HeartPosZKey);

        Debug.Log("Progreso eliminado.");
    }

    //NO ES NECESARIO ESTO, SOLO PARA TESTEOS
    //private void OnApplicationQuit()
    //{
        //ClearProgress();
        //Debug.Log("Progreso borrado al cerrar el juego");
    //}


    private void OnTriggereEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyAir"))
        {
            ClearProgress();

            Debug.Log("Progreso borrado al entrar en el teleport.");
        }
    }

}
