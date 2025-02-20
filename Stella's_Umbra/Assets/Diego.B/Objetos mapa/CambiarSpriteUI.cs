using UnityEngine;
using UnityEngine.UI;

public class CambiarSpriteUI : MonoBehaviour
{
    public GameObject Marcador;

    private void Start()
    {
       Marcador.SetActive(false);
    }

    public void CambiarSprite()
    {
        Marcador.SetActive(true);
    }

    public void RestaurarSprite()
    {
        Marcador.SetActive(false);
    }
}