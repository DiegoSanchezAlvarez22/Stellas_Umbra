using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private GameObject enemyPrefab;

    void Awake()
    {
        // Guardar la posici�n y rotaci�n inicial
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Guardar el prefab original
        enemyPrefab = gameObject;

        // Registrar este enemigo en el EnemyManager
        EnemyManager.Instance.RegisterEnemy(this);
    }

    public void OnPlayerDeath()
    {
        // Notificar al EnemyManager para que lo reinstancie despu�s de 1.5 segundos
        EnemyManager.Instance.RespawnEnemy(enemyPrefab, initialPosition, initialRotation);

        // Destruir el enemigo actual
        Destroy(gameObject);
    }
}


