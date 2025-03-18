using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    private bool _isPlayerIn = false;
    private PlayerInput _input;
    private Transform _playerTransform;

    [SerializeField] InteractableBoss _interactableBoss;
    [SerializeField] EnemyLifes _enemyLifes;
    [SerializeField] Vector3 _newPlayerPosition;
    [SerializeField] GameObject _bossScene;

    [Header("Boss Settings")]
    [SerializeField] GameObject _newBossPrefab; //Prefab del Boss a instanciar
    [SerializeField] GameObject _currentBossInstance; //Instancia del Boss actual
    [SerializeField] private Transform _bossSpawnPoint; // Punto de aparici�n del Boss

    private Vector3 _lastBossPosition; // Variable para guardar la �ltima posici�n del Boss

    private void Start()
    {
        // Inicializamos la posici�n como un valor que no es v�lido (por ejemplo, Vector3.zero)
        _lastBossPosition = Vector3.zero;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerIn = true;
            _input = other.GetComponent<PlayerInput>(); //Obtener PlayerInput del jugador
            _playerTransform = other.transform; //Guarda referencia al transform del player

            if (_input != null)
            {
                _input.actions["Interact"].performed += Interact; // Suscribirse al evento
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerIn = false;

            if (_input != null)
            {
                _input.actions["Interact"].performed -= Interact; // Desuscribirse del evento
                _input = null;
            }

            _playerTransform = null; //Elimina la referencia al transform
            Debug.Log("Saliste de la zona de interacci�n.");
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (_isPlayerIn && _playerTransform != null)
        {
            AudioManagerBehaviour.instance.PlaySFX("Interact");

            Debug.Log("Interacci�n realizada con el objeto.");

            // Hacer visible la escena del Boss
            _bossScene.SetActive(true);
            AudioManagerBehaviour.instance.PlayBossMusic(); // Cambiar m�sica al interactuar

            // Mover al jugador a la nueva posici�n
            _playerTransform.position = _newPlayerPosition;

            // Instanciar el Boss en la misma posici�n que la �ltima vez
            if (_newBossPrefab != null)
            {
                // Si el Boss ya ha sido instanciado, destr�yelo antes de crear uno nuevo
                if (_currentBossInstance != null)
                {
                    Destroy(_currentBossInstance);
                }

                // Si es la primera vez que se instancia, guardamos la posici�n
                if (_lastBossPosition == Vector3.zero)
                {
                    _lastBossPosition = _bossSpawnPoint.position; // Guardamos la posici�n del primer Boss
                }

                // Instanciamos el Boss en la �ltima posici�n registrada con una rotaci�n de 180 grados en Y
                _currentBossInstance = Instantiate(_newBossPrefab, _lastBossPosition, Quaternion.Euler(0f, 180f, 0f));
                _currentBossInstance.transform.localScale = new Vector3(0.34f, 0.34f, 0.34f); // Ajustamos el tama�o
                Debug.Log("Boss instanciado en la posici�n: " + _lastBossPosition);

                // Aqu� llamamos a SetBossInstance para asociar la nueva instancia con el BossManager
                BossManager.Instance.SetBossInstance(_currentBossInstance);
            }

            // Mostrar la vida del Boss
            _interactableBoss.ShowBossCanvasLife();

            // Reseteamos la vida del Boss
            _enemyLifes.ResetBossLifes();
        }
    }

    private void RestartAnimatedBoss()
    {
        Vector3 spawnPosition = Vector3.zero;
        Quaternion spawnRotation = Quaternion.identity;

        //Guardas la posici�n y rotaci�n del Boss antiguo antes de destruirlo
        if (_currentBossInstance != null)
        {
            spawnPosition = _currentBossInstance.transform.position;
            spawnRotation = _currentBossInstance.transform.rotation;

            Destroy(_currentBossInstance);
            Debug.Log("Boss anterior destruido.");
        }
        else
        {
            //Si no hay un Boss previo, tomamos la posici�n y rotaci�n del padre
            spawnPosition = _bossScene.transform.position;
            spawnRotation = _bossScene.transform.rotation;
        }

        //Instancia un nuevo Boss en la misma posici�n y rotaci�n del anterior
        _currentBossInstance = Instantiate(_newBossPrefab, spawnPosition, spawnRotation);

        //Ajustar la escala a (0.34, 0.34, 0.34)
        _currentBossInstance.transform.localScale = new Vector3(0.34f, 0.34f, 0.34f);

        // Guardar referencia en BossManager
        BossManager.Instance.SetBossInstance(_currentBossInstance);

        Debug.Log("Nuevo Boss instanciado en " + spawnPosition + " con escala (0.34, 0.34, 0.34)");
    }
}