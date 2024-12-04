using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int daño; // Cantidad de vida que el enemigo quita al jugador
    [Tooltip("Este valor solo es necesario modificarlo en caso de que se " +
        "quiera destruir el objeto al pasar el valor indicado.")]
    [SerializeField] float _time; // Tiempo que tardará en destruirse si no colisiona con nada

    private void Start()
    {
        if (gameObject.tag == "EnemyShot")
        {
            Invoke("DestroyThis", _time);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            VidaJugador vidaJugador = other.GetComponent<VidaJugador>();

            if (vidaJugador != null)
            {
                vidaJugador.PerderVida(daño); // Quita vida al jugador
                Debug.Log("Jugador recibió daño: " + daño);
            }

            if (gameObject.tag == "EnemyShot")
            {
                DestroyThis();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que colisiona es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            VidaJugador vidaJugador = collision.gameObject.GetComponent<VidaJugador>();

            if (vidaJugador != null)
            {
                vidaJugador.PerderVida(daño); // Quita vida al jugador
                Debug.Log("Jugador recibió daño: " + daño);
            }

            if (gameObject.tag == "EnemyShot")
            {
                DestroyThis();
            }
        }
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
