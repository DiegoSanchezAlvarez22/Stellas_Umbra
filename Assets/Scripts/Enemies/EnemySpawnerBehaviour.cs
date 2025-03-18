using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class EnemySpawnerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab; // El prefab que se va a instanciar
    [SerializeField] PlayerLife _playerLife;

    private void Update()
    {
        DestroyChild();
    }


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

    private void DestroyChild()
    {
        if (_playerLife != null && _playerLife.ActualLife <= 0)
        {
            if (transform.childCount > 0) // Verificar si hay hijos
            {
                Transform child = transform.GetChild(0); // Obtener el primer hijo
                Destroy(child.gameObject); // Destruir el objeto hijo
                Debug.Log("Hijo destruido.");
            }
            else
            {
                Debug.Log("No hay hijos para destruir.");
            }
        }
    }
}
