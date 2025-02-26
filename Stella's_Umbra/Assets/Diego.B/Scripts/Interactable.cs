using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    private bool _isPlayerIn = false;
    private PlayerInput _input;
    private Transform _playerTransform;
    [SerializeField] InteractuableBoss _interactuableBoss;
    [SerializeField] Vector3 _newPlayerPosition;


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
            Debug.Log("Saliste de la zona de interacción.");
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (_isPlayerIn && _playerTransform != null)
        {
            AudioManagerBehaviour.instance.PlaySFX("Interact");

            Debug.Log("Interacción realizada con el objeto.");

            AudioManagerBehaviour.instance.PlayBossMusic(); //Cambia la música al interactuar

            //Mueve al Player a la posición de la escena del Boss
            _playerTransform.position = _newPlayerPosition;

            //Muestra la barra de vida del Boss
            _interactuableBoss.ShowBossCanvasLife();
        }
    }
}
