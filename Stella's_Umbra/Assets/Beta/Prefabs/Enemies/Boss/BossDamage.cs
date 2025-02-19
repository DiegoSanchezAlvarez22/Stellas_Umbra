using UnityEngine;
using UnityEngine.InputSystem;

public class BossDamage : MonoBehaviour
{
    [SerializeField] BoxCollider hitBoxJefe;
    [SerializeField] int _damage;

    private void OnTriggerEnter(Collider hitBoxJefe)
    {
        if (hitBoxJefe.gameObject.CompareTag("Player"))
        {
            VidaJugador vidaJugador = gameObject.GetComponent<VidaJugador>();

            if (vidaJugador != null)
            {
                vidaJugador.PerderVida(_damage, gameObject.transform.position); // Quita vida al jugador
                Debug.Log("Jugador recibió daño: " + _damage);
            }
        }
    }

}
