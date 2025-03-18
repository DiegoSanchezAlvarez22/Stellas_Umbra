using UnityEngine;

public class BatAttack : MonoBehaviour
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
            PlayerLife playerLife = other.GetComponent<PlayerLife>();

            if (playerLife != null)
            {
                playerLife.LooseLife(_damage, other.gameObject.transform.position, gameObject.tag); // Quita vida al jugador
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
            PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();

            if (playerLife != null)
            {
                AudioManagerBehaviour.instance.PlaySFX("Bat Attack");
                Debug.Log("Jugador recibió daño: " + _damage);
                playerLife.LooseLife(_damage, collision.GetContact(0).normal, gameObject.tag); // Quita vida al jugador
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
