using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private GameObject enemyPrefab;

    void Awake()
    {
        // Guardar la posición y rotación inicial
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Guardar el prefab original
        enemyPrefab = gameObject;

        // Registrar este enemigo en el EnemyManager
        EnemyManager.Instance.RegisterEnemy(this);
    }

    public void OnPlayerDeath()
    {
        // Notificar al EnemyManager para que lo reinstancie después de 1.5 segundos
        EnemyManager.Instance.RespawnEnemy(enemyPrefab, initialPosition, initialRotation);

        // Destruir el enemigo actual
        Destroy(gameObject);
    }
}


