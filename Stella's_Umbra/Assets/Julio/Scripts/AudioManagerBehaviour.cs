using UnityEngine;

public class AudioManagerBehaviour : MonoBehaviour
{
    public static AudioManagerBehaviour instance;

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
    public AudioClip interactuar;
    public AudioClip salto;
    public AudioClip buttonClick;

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

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
