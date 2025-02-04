using UnityEngine;

public class AudioManagerBehaviour : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    //[SerializeField] AudioClip[] audios;
    public AudioClip background;
    public AudioClip ataqueDistancia;
    public AudioClip ataqueBasico;
    public AudioClip ataquePiedra;
    public AudioClip ataqueTornado;
    public AudioClip aterrizaje;
    public AudioClip correr;
    public AudioClip dash;
    public AudioClip Interactuar;
    public AudioClip salto;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
