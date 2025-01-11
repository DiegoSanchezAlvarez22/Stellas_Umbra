using UnityEngine;

public class AtaqueDesdeArriba : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAir") || other.CompareTag("EnemyFloor"))
        {
            other.GetComponent<EnemyLifes>().DamageRecieved(_damage); //Perdida de vida del enemigo
            Destroy(gameObject); //Destruir este objeto disparable
        }
    }
}
