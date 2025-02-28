using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;  // Referencia al sistema de part�culas
    public float newSpeed = 5f; // Nueva velocidad de las part�culas
    public Light lightSource;
    public float maxIntensity = 5f;       // Intensidad m�xima de la luz
    public float minIntensity = 0f;       // Intensidad m�nima de la luz
    //public float fadeDuration = 1f;


    void Start()
    {
        var main = _particleSystem.main;  // Obtener el m�dulo Main
        main.startSpeed = newSpeed;      // Cambiar la velocidad de emisi�n
    }
    private void Update()
    {
        if (_particleSystem.isPlaying)
        {
            // Aumentar intensidad basado en la cantidad de part�culas activas
            lightSource.intensity = Mathf.Lerp(lightSource.intensity, maxIntensity, Time.deltaTime * 1f);
        }
        else
        {
            // Reducir la intensidad cuando las part�culas se detienen
            lightSource.intensity = Mathf.Lerp(lightSource.intensity, minIntensity, Time.deltaTime * 1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegurar que es el personaje
        {
            _particleSystem.Play(); // Activar part�culas
            //AudioManagerBehaviour.instance.PlayShineMusic("Shine");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _particleSystem.Stop(); // Detener part�culas
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

        musicSource.Stop(); // Detener la m�sica despu�s de desvanecer el volumen
        musicSource.volume = startVolume; // Restablecer el volumen a su valor original
    }*/
}
