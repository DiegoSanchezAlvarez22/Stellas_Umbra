using UnityEngine;

public class InstantiatePlayer : MonoBehaviour
{
    private static bool _isInstantiated = false; // Variable estática para rastrear si el objeto ya ha sido instanciado

    public GameObject _player; // Asignar al player que se va a instanciar

    void Awake()
    {
        if (!_isInstantiated) // Si el objeto no ha sido instanciado
        {
            Instantiate(_player); // Instancia el objeto
            DontDestroyOnLoad(_player); // Evita que se destruya al cambiar de escena
            _isInstantiated = true; // Marca como instanciado
        }
    }
}
