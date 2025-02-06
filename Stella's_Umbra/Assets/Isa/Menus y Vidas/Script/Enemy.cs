using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int _damage; // Cantidad de vida que el enemigo quita al jugador
    [Tooltip("Este valor solo es necesario modificarlo en caso de que se " +
        "quiera destruir el objeto al pasar el valor indicado.")]

    private void Start()
    {
        if (gameObject.tag == "EnemyShot")
        {
            float _time = 8; // Tiempo que tardará en destruirse si no colisiona con nada
            Invoke("DestroyThis", _time);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que colisiona es el jugador
        {
            VidaJugador vidaJugador = other.GetComponent<VidaJugador>();

            if (vidaJugador != null)
            {
                vidaJugador.PerderVida(_damage); // Quita vida al jugador
                Debug.Log("Jugador recibió daño: " + _damage);
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
                Debug.Log("Jugador recibió daño: " + _damage);
                vidaJugador.PerderVida(_damage); // Quita vida al jugador
            }
        }
    }
}
