using UnityEngine;

public class TornadoBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAir") || other.CompareTag("EnemyFloor"))
        {
            Destroy(other.gameObject);
            Debug.Log("Bala destruida");
        }
    }
}
