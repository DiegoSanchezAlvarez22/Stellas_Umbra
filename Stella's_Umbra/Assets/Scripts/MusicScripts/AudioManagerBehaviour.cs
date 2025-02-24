using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerBehaviour : MonoBehaviour
{
    public static AudioManagerBehaviour instance;

    [Header("---------- Audio Source ----------")]

    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]

    [SerializeField] public AudioClip background;
    [SerializeField] public AudioClip bossBackground;
    [SerializeField] public AudioClip adventureBackground;
    [SerializeField] public List<AudioClip> soundEffects;
    private bool isBossFight = false;
    private bool isLevel1 = false;

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
                isLevel1 = false; // Resetear nivel 1 si estaba activo

                musicSource.Stop();
                musicSource.clip = bossBackground;
                musicSource.Play();
            }
        }
        else if (scene.name == "DiseñoNivel1.1")
        {
            if (!isLevel1)
            {
                isLevel1 = true;
                isBossFight = false; // Resetear boss fight si estaba activo

                musicSource.Stop();
                musicSource.clip = adventureBackground;
                musicSource.Play();
            }
        }
        else if (scene.name == "Menu Principal")
        {
            if (isBossFight || isLevel1) // Si venimos de otra música específica, restablecemos a background
            {
                isBossFight = false;
                isLevel1 = false;

                musicSource.Stop();
                musicSource.clip = background;
                musicSource.Play();
            }
        }
    }
}
