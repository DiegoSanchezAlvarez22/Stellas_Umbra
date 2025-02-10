using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;  // Referencia al sistema de partículas
    public float newSpeed = 5f; // Nueva velocidad de las partículas

    void Start()
    {
        var main = particleSystem.main;  // Obtener el módulo Main
        main.startSpeed = newSpeed;      // Cambiar la velocidad de emisión
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegurar que es el personaje
        {
            particleSystem.Play(); // Activar partículas
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            particleSystem.Stop(); // Detener partículas
        }
    }
}
