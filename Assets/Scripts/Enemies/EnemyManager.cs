using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private List<EnemyRespawn> enemies = new List<EnemyRespawn>();

    void Awake()
    {
        // Asegurar que haya solo una instancia del EnemyManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(EnemyRespawn enemy)
    {
        // Agregar cada enemigo a la lista de control
        enemies.Add(enemy);
    }

    public void RespawnEnemy(GameObject enemyPrefab, Vector3 position, Quaternion rotation)
    {
        StartCoroutine(RespawnCoroutine(enemyPrefab, position, rotation));
    }

    private IEnumerator RespawnCoroutine(GameObject enemyPrefab, Vector3 position, Quaternion rotation)
    {
        yield return new WaitForSeconds(1.5f);
        Instantiate(enemyPrefab, position, rotation);
    }

    // Método que faltaba y causaba el error
    public List<EnemyRespawn> GetEnemies()
    {
        return enemies;
    }
}


