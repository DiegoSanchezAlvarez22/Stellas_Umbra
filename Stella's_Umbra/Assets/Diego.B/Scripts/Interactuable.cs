using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Interactuable : MonoBehaviour
{
    private bool jugadorDentro = false;
    private PlayerInput playerInput;
    public string nombreEscena; // Nombre de la escena a cargar


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
            Debug.Log("Interacción realizada con el objeto.");
            // Aquí puedes poner la lógica de interacción (abrir puerta, recoger objeto, etc.)
            SceneManager.LoadScene(nombreEscena);
        }
    }
}
