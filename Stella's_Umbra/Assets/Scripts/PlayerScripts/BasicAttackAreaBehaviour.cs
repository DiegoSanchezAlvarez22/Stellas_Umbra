using UnityEngine;

public class BasicAttackAreaBehaviour : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyLifes>() != null)
        {
            other.GetComponent<EnemyLifes>().DamageRecieved(_damage); //Perdida de vida del enemigo
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyLifes>() != null)
        {
            collision.gameObject.GetComponent<EnemyLifes>().DamageRecieved(_damage); //Perdida de vida del enemigo
        }
    }
}
