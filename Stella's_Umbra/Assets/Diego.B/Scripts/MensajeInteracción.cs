using UnityEngine;
using UnityEngine.Video;

public class MensajeInteracción : MonoBehaviour
{
    [SerializeField] GameObject Icono;
    [SerializeField] BoxCollider RangoInteracción;

    private void Start()
    {
        Icono.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactuable") || other.CompareTag("Acceso Jefe"))
        {
            Icono.SetActive(true);
            Debug.Log("Puede interactuar");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactuable") || other.CompareTag("Acceso Jefe"))
        {
            Icono.SetActive(false);
        }
    }
}
