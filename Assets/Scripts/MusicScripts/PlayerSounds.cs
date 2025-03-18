using UnityEngine;
using UnityEngine.Audio;

public class PlayerSounds : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips; // Agrega aquí los clips de audio desde el inspector.
    [SerializeField] AudioMixer miMixer; //Referencia al Audio Mixer
    private bool isJumping = false;
    private bool isOnGround = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //Aplica el volumen guardado del Audio Mixer, al AudioSource del Player
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        miMixer.SetFloat("SFX", Mathf.Log10(savedVolume) * 20);
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
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
        {
            isOnGround = true; // El jugador está en el suelo
            if (isJumping) // Solo reproduce el sonido si ha saltado
            {
                PlaySpecificClip(2);
                isJumping = false; // Resetear estado de salto
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Verificar si el jugador deja de tocar el suelo
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
        {
            isOnGround = false; // El jugador ya no está en el suelo
        }
    }

    public void MovingSound()
    {
        if (!audioSource.isPlaying && isOnGround == true)
        {
            PlaySpecificClip(0);
        }
    }

    public void DashingSound()
    {
        PlaySpecificClip(3);
    }

    public void JumpingSound()
    {
        PlaySpecificClip(1);
    }
}
