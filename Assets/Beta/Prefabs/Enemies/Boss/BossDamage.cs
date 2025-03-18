using UnityEngine;
using UnityEngine.InputSystem;

public class BossDamage : MonoBehaviour
{
    [SerializeField] BoxCollider _colliderBoss;
    [SerializeField] int _damage;

    private void OnTriggerEnter(Collider _colliderBoss)
    {
        if (_colliderBoss.gameObject.CompareTag("Player"))
        {
            PlayerLife vidaJugador = gameObject.GetComponent<PlayerLife>();

            if (vidaJugador != null)
            {
                vidaJugador.LooseLife(_damage, gameObject.transform.position, gameObject.tag); // Quita vida al jugador
                Debug.Log("Jugador recibió daño: " + _damage);
            }
        }
    }

}
