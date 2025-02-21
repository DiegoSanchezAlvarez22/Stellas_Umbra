using UnityEngine;

public class MapBehaviour : MonoBehaviour
{
    public Animator _anim;
    public bool _isOpen = false;

    private void Start()
    {
        _anim.Play("MapaCerrado");
    }
    public void ChangeMap()
    {
        _isOpen = !_isOpen;
        if (_isOpen)
        {
            _anim.Play("MapaAbierto");
        }
        else
        {
            _anim.Play("MapaCerrado");
        }
    }
}
