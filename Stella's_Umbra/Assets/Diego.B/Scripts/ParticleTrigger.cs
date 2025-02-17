using TMPro;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;  // Referencia al sistema de partículas
    public float newSpeed = 5f; // Nueva velocidad de las partículas
    public Light lightSource;
    public float maxIntensity = 5f;       // Intensidad máxima de la luz
    public float minIntensity = 0f;       // Intensidad mínima de la luz


    void Start()
    {
        var main = _particleSystem.main;  // Obtener el módulo Main
        main.startSpeed = newSpeed;      // Cambiar la velocidad de emisión
    }
    private void Update()
    {
        if (_particleSystem.isPlaying)
        {
            // Aumentar intensidad basado en la cantidad de partículas activas
            lightSource.intensity = Mathf.Lerp(lightSource.intensity, maxIntensity, Time.deltaTime * 1f);
        }
        else
        {
            // Reducir la intensidad cuando las partículas se detienen
            lightSource.intensity = Mathf.Lerp(lightSource.intensity, minIntensity, Time.deltaTime * 1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegurar que es el personaje
        {
            _particleSystem.Play(); // Activar partículas
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _particleSystem.Stop(); // Detener partículas
        }
    }
}
