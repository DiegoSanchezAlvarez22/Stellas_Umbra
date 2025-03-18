using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteractableTeleports : MonoBehaviour
{
    [SerializeField] private MapBehaviour _mapBehaviour;
    private PlayerInput _input;

    private bool _isPlayerIn = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerIn = true;
            _input = other.GetComponent<PlayerInput>(); // Obtener PlayerInput del jugador

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

            Debug.Log("Saliste de la zona de interacción.");
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (_isPlayerIn)
        {
            _mapBehaviour.ChangeMap();
        }
    }
}
