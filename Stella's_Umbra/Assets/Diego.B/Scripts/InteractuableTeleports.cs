using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteractuableTeleports : MonoBehaviour
{
    private bool jugadorDentro = false;
    private PlayerInput playerInput;
    [SerializeField] private MapaDesplegable mapa;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            playerInput = other.GetComponent<PlayerInput>(); // Obtener PlayerInput del jugador

            if (playerInput != null)
            {
                playerInput.actions["Interact"].performed += Interactuar; // Suscribirse al evento
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;

            if (playerInput != null)
            {
                playerInput.actions["Interact"].performed -= Interactuar; // Desuscribirse del evento
                playerInput = null;
            }

            Debug.Log("Saliste de la zona de interacción.");
        }
    }

    private void Interactuar(InputAction.CallbackContext context)
    {
        if (jugadorDentro)
        {
            mapa.AlternarMapa();
        }
    }
}
