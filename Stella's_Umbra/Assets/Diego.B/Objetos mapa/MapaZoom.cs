using UnityEngine;
using UnityEngine.EventSystems;

public class MapaZoom : MonoBehaviour, IScrollHandler
{
    public RectTransform mapa; // RectTransform del mapa
    public RectTransform contenedor; // RectTransform del contenedor (con la máscara)
    public float zoomSpeed = 0.1f; // Velocidad de zoom
    private float minZoom; // Escala mínima (se definirá dinámicamente)
    public float maxZoom = 2.0f; // Escala máxima

    private void Start()
    {
        // Establece el mínimo nivel de zoom como la escala inicial del mapa
        minZoom = mapa.localScale.x;
    }

    public void OnScroll(PointerEventData eventData)
    {
        // Calcula el nuevo tamaño de escala
        float newScale = Mathf.Clamp(mapa.localScale.x + eventData.scrollDelta.y * zoomSpeed, minZoom, maxZoom);
        mapa.localScale = new Vector3(newScale, newScale, 1);

        // Limita la posición del mapa dentro del contenedor
        LimitarPosicion();
    }

    private void LimitarPosicion()
    {
        // Obtén los bordes del mapa y del contenedor en su espacio local
        Vector3[] contenedorCorners = new Vector3[4];
        Vector3[] mapaCorners = new Vector3[4];
        contenedor.GetWorldCorners(contenedorCorners);
        mapa.GetWorldCorners(mapaCorners);

        // Calcula los límites del contenedor en el espacio local del mapa
        Vector3 min = contenedor.InverseTransformPoint(contenedorCorners[0]);
        Vector3 max = contenedor.InverseTransformPoint(contenedorCorners[2]);

        // Asegúrate de que los bordes del mapa no salgan del contenedor
        Vector3 mapaPos = mapa.localPosition;
        float mapaWidth = mapa.rect.width * mapa.localScale.x;
        float mapaHeight = mapa.rect.height * mapa.localScale.y;

        mapaPos.x = Mathf.Clamp(mapaPos.x, min.x - (mapaWidth / 2), max.x + (mapaWidth / 2));
        mapaPos.y = Mathf.Clamp(mapaPos.y, min.y - (mapaHeight / 2), max.y + (mapaHeight / 2));

        mapa.localPosition = mapaPos;
    }
}
