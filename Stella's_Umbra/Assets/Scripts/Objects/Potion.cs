using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] int heal; 

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            PlayerLife vidaJugador = other.GetComponent<PlayerLife>();

            if (vidaJugador != null)
            {
                vidaJugador.IncreaseLife(heal); // Quita vida al jugador
                Debug.Log("Jugador recibió una cura: " + heal);
            }
        }
    }
}
