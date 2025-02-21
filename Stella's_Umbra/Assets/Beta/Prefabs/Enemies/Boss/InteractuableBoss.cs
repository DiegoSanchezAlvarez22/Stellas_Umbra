using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteractuableBoss : MonoBehaviour
{
    private bool jugadorDentro = false;
    private PlayerInput playerInput;
    [SerializeField] BoxCollider interactuar;
    [SerializeField] Animator Boss;

    private void OnTriggerEnter(Collider interactuar)
    {
        if (interactuar.CompareTag("Player"))
        {
            jugadorDentro = true;
            playerInput = interactuar.GetComponent<PlayerInput>(); // Obtener PlayerInput del jugador

            if (playerInput != null)
            {
                playerInput.actions["Interact"].performed += Interactuar; // Suscribirse al evento
            }
        }
    }

    private void OnTriggerExit(Collider interactuar)
    {
        if (interactuar.CompareTag("Player"))
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
            Boss.SetTrigger("Awake");
            StartCoroutine(FasesJefe());  // Llamada correcta a la corrutina
            interactuar.enabled = false; // Desactivar el collider después de la interacción
        }
    }

    private IEnumerator FasesJefe()
    {
        yield return new WaitForSeconds(5f); // Espera el tiempo de la animación

        Boss.SetTrigger("Barrido");

        yield return new WaitForSeconds(5f); // Espera el tiempo de la animación

        Boss.SetTrigger("Palmada");

        yield return new WaitForSeconds(5f); // Espera el tiempo de la animación

    }
}
