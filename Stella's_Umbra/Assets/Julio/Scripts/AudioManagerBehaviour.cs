using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerBehaviour : MonoBehaviour
{
    public static AudioManagerBehaviour instance;

    [Header("---------- Audio Source ----------")]

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]

    [SerializeField] public AudioClip background;
    [SerializeField] public AudioClip bossBackground;
    [SerializeField] public List<AudioClip> soundEffects;

    private bool isBossFight = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Suscribirse al evento de carga de escena
        }
        else
        {
            //Destruye el nuevo AudioManager si ya existe uno
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Desuscribirse para evitar errores
    }

    public void PlaySFX(string soundName)
    {
        foreach (var clip in soundEffects)
        {
            if (clip.name == soundName)
            {
                SFXSource.PlayOneShot(clip);
                return;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BossRoom")
        {
            if (!isBossFight)
            {
                isBossFight = true;

                musicSource.clip = background;
                musicSource.Stop();

                musicSource.clip = bossBackground;
                musicSource.Play();
            }
        }
        else
        {
            //Si venimos de la escena del jefe, restauramos la música
            if (isBossFight)
            {
                isBossFight = false;
                musicSource.clip = bossBackground;
                musicSource.Stop();

                musicSource.clip = background;
                musicSource.Play();
            }
        }
    }
}
