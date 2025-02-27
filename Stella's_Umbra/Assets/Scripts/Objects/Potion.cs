using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] int heal; 

    private void OnTriggerEnter(Collider other)
    {
        //Verifica si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            PlayerLife vidaJugador = other.GetComponent<PlayerLife>();

            if (vidaJugador != null)
            {
                vidaJugador.IncreaseLife(heal); //Aumenta vida al jugador
                AudioManagerBehaviour.instance.PlaySFX("Take Item"); //Sonido del item
                Debug.Log("Jugador recibió una cura: " + heal);
                Destroy(this.gameObject);
            }
        }
    }
}
