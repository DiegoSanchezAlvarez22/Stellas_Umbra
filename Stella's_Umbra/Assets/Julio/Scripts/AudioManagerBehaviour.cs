using System.Collections.Generic;
using UnityEngine;

public class AudioManagerBehaviour : MonoBehaviour
{
    public static AudioManagerBehaviour instance;

    [Header("---------- Audio Source ----------")]

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]

    [SerializeField] public AudioClip background;
    [SerializeField] public List<AudioClip> soundEffects;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
}
