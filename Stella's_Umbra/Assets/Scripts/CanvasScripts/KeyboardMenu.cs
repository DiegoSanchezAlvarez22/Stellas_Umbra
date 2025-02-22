using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject _objectToActivate;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_objectToActivate != null)
            _objectToActivate.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_objectToActivate != null)
            _objectToActivate.SetActive(false);
    }
}