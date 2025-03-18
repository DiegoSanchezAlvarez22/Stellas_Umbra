using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDestroyLoads : MonoBehaviour
{
    [SerializeField] private string[] _scenes; // Lista de escenas donde el objeto persiste

    [System.Obsolete]
    private void Awake()
    {
        // Si hay otro objeto igual en la escena, lo destruimos para evitar duplicados
        if (FindObjectsOfType<PlayerDestroyLoads>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Mantener el objeto si la escena actual está en la lista
        if (_scenes.Contains(SceneManager.GetActiveScene().name))
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Si cambiamos a una escena que no está en la lista, destruir el objeto
        if (!_scenes.Contains(SceneManager.GetActiveScene().name))
        {
            Destroy(gameObject);
        }
    }
}
