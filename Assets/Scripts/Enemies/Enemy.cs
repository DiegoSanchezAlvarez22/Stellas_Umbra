using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Este valor solo es necesario modificarlo en caso de que se " +
        "quiera destruir el objeto al pasar el valor indicado.")]
    [SerializeField] int _damage; // Cantidad de vida que el enemigo quita al jugador
    private EnemyLifes _enemyLifes;

    private void Start()
    {
        _enemyLifes = gameObject.GetComponent<EnemyLifes>();

        if (gameObject.tag == "EnemyShot")
        {
            float _time = 8; // Tiempo que tardar� en destruirse si no colisiona con nada
            Invoke("DestroyThis", _time);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Verifica si el objeto que colisiona es el jugador
        {
            PlayerLife playerLife = other.GetComponent<PlayerLife>();

            if (gameObject.CompareTag("Boss"))
            {
                if (playerLife != null && _enemyLifes.isDead == false)
                {
                    if (gameObject.tag == "EnemyShot")
                        playerLife.LooseLife(_damage, other.gameObject.transform.position, gameObject.tag); // Quita vida al jugador
                    Debug.Log("Jugador recibi� da�o: " + _damage);
                }
            }
            if (playerLife != null)
            {
                if (gameObject.tag == "EnemyShot")
                playerLife.LooseLife(_damage, other.gameObject.transform.position, gameObject.tag); // Quita vida al jugador
                Debug.Log("Jugador recibi� da�o: " + _damage);
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
            PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();

            if (gameObject.CompareTag("Boss"))
            {
                if (playerLife != null && _enemyLifes.isDead == false)
                {
                    Debug.Log("Jugador recibi� da�o: " + _damage);
                    playerLife.LooseLife(_damage, collision.GetContact(0).normal, gameObject.tag); // Quita vida al jugador
                }
            }
            else
            {
                if (playerLife != null)
                {
                    Debug.Log("Jugador recibi� da�o: " + _damage);
                    playerLife.LooseLife(_damage, collision.GetContact(0).normal, gameObject.tag); // Quita vida al jugador
                }
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
