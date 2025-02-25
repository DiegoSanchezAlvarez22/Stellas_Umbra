using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class InteractuableBoss : MonoBehaviour
{
    private bool jugadorDentro = false;
    private PlayerInput playerInput;
    [SerializeField] BoxCollider interact;
    [SerializeField] BoxCollider hitBox;
    [SerializeField] Animator Boss;
    [SerializeField] VideoPlayer canvaPostMortem;
    [SerializeField] GameObject sistemaParticulas;

    private void Start()
    {
        hitBox.enabled = false;
        sistemaParticulas.SetActive(false); // Desactiva completamente el objeto
    }

    private void Update()
    {
        if (Boss.GetCurrentAnimatorStateInfo(0).IsName("Armature|MuerteIdle"))
        {
            interact.enabled = true; // Desactivar el collider despu�s de la interacci�n
            hitBox.enabled = false;
            StopCoroutine(FasesJefe());
            sistemaParticulas.SetActive(true); // Activa el objeto y las part�culas
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            playerInput = other.GetComponent<PlayerInput>(); // Obtener PlayerInput del jugador

            // Aqu� puedes poner la l�gica de interacci�n (abrir puerta, recoger objeto, etc.)
            Boss.SetTrigger("Awake");
            StartCoroutine(FasesJefe());  // Llamada correcta a la corrutina
            interact.enabled = false; // Desactivar el collider despu�s de la interacci�n

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
        }
    }

    private void Interactuar(InputAction.CallbackContext context)
    {
        if (!jugadorDentro) return; // Si el jugador no est� dentro, no hacer nada

        if (Boss.GetCurrentAnimatorStateInfo(0).IsName("Armature|MuerteIdle"))
        {
            canvaPostMortem.Play();
        }
    }

    private IEnumerator FasesJefe()
    {
        yield return new WaitForSeconds(1f); // Espera el tiempo de la animaci�n
                                             
        hitBox.enabled = true;

        while (true)
        {
            yield return new WaitForSeconds(5f); // Espera el tiempo de la animaci�n

            Boss.SetTrigger("Barrido");

            yield return new WaitForSeconds(5f); // Espera el tiempo de la animaci�n

            Boss.SetTrigger("Palmada");

            yield return new WaitForSeconds(2f); // Espera el tiempo de la animaci�n

            Boss.SetTrigger("Palmada");

            yield return new WaitForSeconds(10f); // Espera el tiempo de la animaci�n

            Boss.SetTrigger("Barrido");

            yield return new WaitForSeconds(2f); // Espera el tiempo de la animaci�n

            Boss.SetTrigger("Palmada");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verificamos si el objeto con el que colisionamos tiene el tag "Enemigo"
        if (collision.gameObject.CompareTag("Floor") && Boss.GetCurrentAnimatorStateInfo(0).IsName("Armature|Palmada"))
        {
            Debug.Log("Hola");
            OndaExpansiva();
        }
    }


    private void OndaExpansiva()
    {

    }
}
