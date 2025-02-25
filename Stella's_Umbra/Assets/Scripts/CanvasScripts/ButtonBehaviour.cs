using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    private Animator _anim;
    private RectTransform _rectTransform;
    void Start()
    {
        _anim = GetComponent<Animator>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void ChangeAnimState()
    {
        _rectTransform.localScale = Vector3.one;
        _rectTransform.rotation = Quaternion.EulerAngles(0,0,0);
        _anim.SetTrigger("Normal");
    }
}
