using UnityEngine;
using UnityEngine.Video;

public class MensajeInteracción : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] BoxCollider RangoInteracción;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactuable") || other.CompareTag("Acceso Jefe"))
        {
            videoPlayer.Play();
            Debug.Log("Puede interactuar");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactuable") || other.CompareTag("Acceso Jefe"))
        {
            videoPlayer.Stop();
        }
    }
}
