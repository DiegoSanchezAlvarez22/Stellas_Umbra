using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Este valor solo es necesario modificarlo en caso de que se " +
        "quiera destruir el objeto al pasar el valor indicado.")]
    [SerializeField] int _damage; // Cantidad de vida que el enemigo quita al jugador

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
        if (other.gameObject.CompareTag("Player")) // Verifica si el objeto que colisiona es el jugador
        {
            VidaJugador vidaJugador = other.GetComponent<VidaJugador>();

            if (vidaJugador != null)
            {
                vidaJugador.PerderVida(_damage, other.gameObject.transform.position); // Quita vida al jugador
                Debug.Log("Jugador recibió daño: " + _damage);
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
                Debug.Log("Jugador recibió daño: " + _damage);
                vidaJugador.PerderVida(_damage, collision.GetContact(0).normal); // Quita vida al jugador
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
