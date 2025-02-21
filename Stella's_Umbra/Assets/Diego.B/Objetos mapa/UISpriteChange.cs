using UnityEngine;
using UnityEngine.UI;

public class UISpriteChange : MonoBehaviour
{
    public GameObject _marker;

    private void Start()
    {
       _marker.SetActive(false);
    }

    public void ChangeSprite()
    {
        _marker.SetActive(true);
    }

    public void RestartSprite()
    {
        _marker.SetActive(false);
    }
}