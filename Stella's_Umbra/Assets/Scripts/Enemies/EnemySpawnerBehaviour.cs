using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class EnemySpawnerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab; // El prefab que se va a instanciar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transform.childCount == 0)
        {
            Instantiate();
        }
    }

    void Instantiate()
    {
        if (_enemyPrefab != null)
        {
            GameObject nuevoObjeto = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            nuevoObjeto.transform.SetParent(transform); // Convertirlo en hijo del objeto actual
        }
        else
        {
            Debug.LogWarning("No se ha asignado un prefab para instanciar.");
        }
    }
}
