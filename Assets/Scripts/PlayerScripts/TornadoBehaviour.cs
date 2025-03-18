using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoBehaviour : MonoBehaviour
{
    //[SerializeField] private float _damage;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("EnemyAir") || other.CompareTag("EnemyFloor"))
    //    {
    //        other.GetComponent<EnemyLifes>().DamageRecieved(_damage); //Perdida de vida del enemigo
    //    }
    //}

    [SerializeField] private float damage = 10f; // Daño que hace el tornado
    [SerializeField] private float damageInterval = 1f; // Cada cuánto tiempo hace daño
    private List<EnemyLifes> enemiesInTornado = new List<EnemyLifes>();

    private void OnTriggerEnter(Collider other)
    {
        EnemyLifes _enemyLifes = other.GetComponent<EnemyLifes>();
        if (_enemyLifes != null)
        {
            if (!enemiesInTornado.Contains(_enemyLifes))
            {
                enemiesInTornado.Add(_enemyLifes);
                StartCoroutine(DamageOverTime(_enemyLifes));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyLifes _enemyLifes = other.GetComponent<EnemyLifes>();
        if (_enemyLifes != null && enemiesInTornado.Contains(_enemyLifes))
        {
            enemiesInTornado.Remove(_enemyLifes);
        }
    }

    private IEnumerator DamageOverTime(EnemyLifes _enemyLifes)
    {
        while (enemiesInTornado.Contains(_enemyLifes))
        {
            _enemyLifes.DamageRecieved(damage);
            yield return new WaitForSeconds(damageInterval);
        }
    }

}
