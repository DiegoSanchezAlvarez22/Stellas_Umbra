using UnityEngine;
using UnityEngine.Video;

public class InteractionMesage : MonoBehaviour
{
    [SerializeField] GameObject _icon;
    [SerializeField] BoxCollider _interactionCollider;

    bool wasIconActive = false;

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

    public void OnGamePaused()
    {
        wasIconActive = _icon.activeSelf; //Guarda si estaba activo antes de pausar

        _icon.SetActive(false); //Oculta el icono al pausar
    }

    public void OnGameResumed()
    {
        _icon.SetActive(wasIconActive); //Restaura el estado previo al reanudar
    }
}
