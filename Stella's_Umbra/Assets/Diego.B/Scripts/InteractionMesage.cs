using UnityEngine;
using UnityEngine.Video;

public class InteractionMesage : MonoBehaviour
{
    [SerializeField] GameObject _icon;
    [SerializeField] BoxCollider _interactionCollider;

    private void Start()
    {
        _icon.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactuable") || other.CompareTag("Acceso Jefe"))
        {
            _icon.SetActive(true);
            Debug.Log("Puede interactuar");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactuable") || other.CompareTag("Acceso Jefe"))
        {
            _icon.SetActive(false);
        }
    }
}
