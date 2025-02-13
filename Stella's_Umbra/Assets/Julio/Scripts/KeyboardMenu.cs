using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject objetoAActivar;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (objetoAActivar != null)
            objetoAActivar.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (objetoAActivar != null)
            objetoAActivar.SetActive(false);
    }
}