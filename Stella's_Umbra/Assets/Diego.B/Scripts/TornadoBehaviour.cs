using UnityEngine;

public class TornadoBehaviour : MonoBehaviour
{
    [SerializeField] private float _damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAir") || other.CompareTag("EnemyFloor"))
        {
            other.GetComponent<EnemyLifes>().DamageRecieved(_damage); //Perdida de vida del enemigo
        }
    }
}
