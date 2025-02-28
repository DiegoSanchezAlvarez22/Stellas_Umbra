using UnityEngine;
using UnityEngine.EventSystems;

public class MapaZoom : MonoBehaviour, IScrollHandler
{
    public RectTransform mapa; // RectTransform del mapa
    public RectTransform contenedor; // RectTransform del contenedor (con la m�scara)
    public float zoomSpeed = 0.1f; // Velocidad de zoom
    private float minZoomx; // Escala m�nima (se definir� din�micamente)
    private float minZoomy; // Escala m�nima (se definir� din�micamente)
    public float maxZoom = 2.0f; // Escala m�xima

    private void Start()
    {
        // Establece el m�nimo nivel de zoom como la escala inicial del mapa
        minZoomx = mapa.localScale.x;
        minZoomy = mapa.localScale.y;
    }

    public void OnScroll(PointerEventData eventData)
    {
        // Calcula el nuevo tama�o de escala
        float newScalex = Mathf.Clamp(mapa.localScale.x + eventData.scrollDelta.y * zoomSpeed, minZoomx, maxZoom);
        float newScaley = Mathf.Clamp(mapa.localScale.y + eventData.scrollDelta.y * zoomSpeed, minZoomy, maxZoom);
        mapa.localScale = new Vector3(newScalex, newScaley, 1);

        // Limita la posici�n del mapa dentro del contenedor
        LimitarPosicion();
    }

    private void LimitarPosicion()
    {
        // Obt�n los bordes del mapa y del contenedor en su espacio local
        Vector3[] contenedorCorners = new Vector3[4];
        Vector3[] mapaCorners = new Vector3[4];
        contenedor.GetWorldCorners(contenedorCorners);
        mapa.GetWorldCorners(mapaCorners);

        // Calcula los l�mites del contenedor en el espacio local del mapa
        Vector3 min = contenedor.InverseTransformPoint(contenedorCorners[0]);
        Vector3 max = contenedor.InverseTransformPoint(contenedorCorners[2]);

        // Aseg�rate de que los bordes del mapa no salgan del contenedor
        Vector3 mapaPos = mapa.localPosition;
        float mapaWidth = mapa.rect.width * mapa.localScale.x;
        float mapaHeight = mapa.rect.height * mapa.localScale.y;

        mapaPos.x = Mathf.Clamp(mapaPos.x, min.x - (mapaWidth / 2), max.x + (mapaWidth / 2));
        mapaPos.y = Mathf.Clamp(mapaPos.y, min.y - (mapaHeight / 2), max.y + (mapaHeight / 2));

        mapa.localPosition = mapaPos;
    }
}
