using UnityEngine;
using UnityEngine.EventSystems;

public class MapaZoom : MonoBehaviour, IScrollHandler
{
    public RectTransform mapa; // El RectTransform del mapa
    public float zoomSpeed = 0.1f; // Velocidad de zoom
    public float minZoom = 0.5f; // Escala mínima
    public float maxZoom = 2.0f; // Escala máxima

    public void OnScroll(PointerEventData eventData)
    {
        // Calcula la nueva escala basada en la dirección del scroll
        float newScale = Mathf.Clamp(mapa.localScale.x + eventData.scrollDelta.y * zoomSpeed, minZoom, maxZoom);

        // Aplica la escala uniformemente en X e Y
        mapa.localScale = new Vector3(newScale, newScale, 1);
    }
}
