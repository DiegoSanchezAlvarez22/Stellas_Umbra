using UnityEngine;
using UnityEngine.EventSystems;

public class ImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject _explanation;

    private void Awake()
    {
        _explanation = transform.Find("Explanation").gameObject; //Busca automáticamente el hijo que actúa como tooltip

        //Tiene que estar desactivado al inicio
        if (_explanation != null)
        {
            _explanation.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No se encontró un Tooltip como hijo de " + gameObject.name);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_explanation != null)
        {
            _explanation.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_explanation != null)
        {
            _explanation.SetActive(false);
        }
    }
}
