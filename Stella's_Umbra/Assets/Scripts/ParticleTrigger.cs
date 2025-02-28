using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;  // Referencia al sistema de partículas
    public float newSpeed = 5f; // Nueva velocidad de las partículas
    public Light lightSource;
    public float maxIntensity = 5f;       // Intensidad máxima de la luz
    public float minIntensity = 0f;       // Intensidad mínima de la luz
    //public float fadeDuration = 1f;


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
            //AudioManagerBehaviour.instance.PlayShineMusic("Shine");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _particleSystem.Stop(); // Detener partículas
            //StartCoroutine(FadeOutMusic());
        }
    }

   /*private IEnumerator FadeOutMusic()
    {
        AudioSource musicSource = AudioManagerBehaviour.instance.musicSource;
        float startVolume = musicSource.volume;

        // Disminuir el volumen gradualmente
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration; // Reducir el volumen
            yield return null; // Esperar hasta el siguiente frame
        }

        musicSource.Stop(); // Detener la música después de desvanecer el volumen
        musicSource.volume = startVolume; // Restablecer el volumen a su valor original
    }*/
}
