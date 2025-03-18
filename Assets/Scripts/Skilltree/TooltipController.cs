using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{
    [SerializeField] private Text tooltipText; // Texto del tooltip
    [SerializeField] private RectTransform tooltipBackground; // Fondo del tooltip

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip(string description, Vector2 position)
    {
        tooltipText.text = description;
        tooltipBackground.sizeDelta = new Vector2(tooltipText.preferredWidth + 20, tooltipText.preferredHeight + 20);
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
