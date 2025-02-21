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
            PlayerLife itemCollector = other.GetComponent<PlayerLife>();

            if (itemCollector != null)
            {
                itemCollector.TakeItem();
                Destroy(gameObject); // Destruye el item después de recogerlo
            }
        }
    }
}
