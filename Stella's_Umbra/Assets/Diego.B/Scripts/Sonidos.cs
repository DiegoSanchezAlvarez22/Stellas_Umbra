using UnityEngine;
using UnityEngine.Audio;

public class Sonidos : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips; // Agrega aquí los clips de audio desde el inspector.
    [SerializeField] AudioMixer miMixer; //Referencia al Audio Mixer
    private bool isJumping = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //JULIO Aplica el volumen guardado del Audio Mixer, al AudioSource del Player
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        miMixer.SetFloat("SFX", Mathf.Log10(savedVolume) * 20);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (!audioSource.isPlaying)
            {
                PlaySpecificClip(0);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                PlaySpecificClip(3);
            }
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (!audioSource.isPlaying)
            {
                PlaySpecificClip(1);
                isJumping = true;
            }
        }
        else
        {
            // Detener el sonido si no se presiona ninguna tecla relevante
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
    private void PlaySpecificClip(int index)
    {
        if (index >= 0 && index < audioClips.Length)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si el personaje choca contra el suelo
        if (collision.gameObject.CompareTag("Floor") && isJumping)
        {
            PlaySpecificClip(2);
            isJumping = false; // Resetear estado de salto
        }
    }

}

