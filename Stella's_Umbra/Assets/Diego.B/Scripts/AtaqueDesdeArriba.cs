using UnityEngine;

public class AtaqueDesdeArriba : MonoBehaviour
{
    [SerializeField] int damage;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAir") || other.CompareTag("EnemyFloor"))
        {
            Destroy(other.gameObject);
            Debug.Log("Bala destruida");
        }
    }
}
