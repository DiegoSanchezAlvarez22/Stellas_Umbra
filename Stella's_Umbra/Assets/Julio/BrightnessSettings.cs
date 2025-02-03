using UnityEngine;
using UnityEngine.UI;

public class BrightnessSettings : MonoBehaviour
{
    [SerializeField] Image brightnessPanel;
    [SerializeField] Slider brightnessSlider;


    void Start()
    {
        if (PlayerPrefs.HasKey("brightness"))
        {
            LoadBrightness();
        }
        else
        {
            SetBrightness();
        }
    }

    // Update is called once per frame
    public void SetBrightness()
    {
        float brightnessValue = brightnessSlider.value;

        //Ajusta la opacidad del panel según el brillo
        Color panelColor = brightnessPanel.color;
        //Ajusta la opacidad del panel 1 opaco 0 transparente
        panelColor.a = Mathf.Lerp(0f, 0.9f, 1 - brightnessValue);
        brightnessPanel.color = panelColor;

        //Guarda el valor del brillo en PlayerPrefs
        PlayerPrefs.SetFloat("brightness", brightnessValue);
    }


    public void LoadBrightness()
    {
        float brightnessValue = PlayerPrefs.GetFloat("brightness");
        brightnessSlider.value = brightnessValue;

        //Aplica el valor cargado
        SetBrightness();
    }
}
