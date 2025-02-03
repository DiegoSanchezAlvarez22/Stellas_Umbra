using UnityEngine;
using UnityEngine.UI;

public class ApplyBrightness : MonoBehaviour
{

    [SerializeField] Image brightnessPanel;

    //Rango de opacidad que debe coincidir con el rango definido en BrightnessSettings script
    float minOpacity = 0f;
    float maxOpacity = 0.9f;

    void Start()
    {
        ApplySavedBrightness();
    }

    private void Update()
    {
        ApplySavedBrightness();
    }


    private void ApplySavedBrightness()
    {
        if (PlayerPrefs.HasKey("brightness"))
        {
            float brightnessValue = PlayerPrefs.GetFloat("brightness");

            //Ajusta la opacidad del panel según el valor guardado
            Color panelColor = brightnessPanel.color;
            panelColor.a = Mathf.Lerp(minOpacity, maxOpacity, 1 - brightnessValue);
            brightnessPanel.color = panelColor;
        }
        else
        {
            Debug.LogWarning("No se encontró un valor guardado para el brillo");
        }
    }
}
