using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            VidaJugador itemCollector = other.GetComponent<VidaJugador>();

            if (itemCollector != null)
            {
                itemCollector.RecogerItem();
                Destroy(gameObject); // Destruye el item después de recogerlo
            }
        }
    }
}
