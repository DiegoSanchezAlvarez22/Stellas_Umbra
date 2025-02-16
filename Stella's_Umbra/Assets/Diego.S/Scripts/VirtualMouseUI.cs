using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseUI : MonoBehaviour
{
    private VirtualMouseInput _virtualMouseInput;
    [SerializeField] private RectTransform _canvasRectTransform;

    private void Awake()
    {
        _virtualMouseInput = GetComponent<VirtualMouseInput>();
    }

    private void Update()
    {
        transform.localScale = Vector3.one * (1f / _canvasRectTransform.localScale.x);
        transform.SetAsLastSibling();
    }

    private void LateUpdate()
    {
        Vector2 _virtualMousePos = _virtualMouseInput.virtualMouse.position.value;
        _virtualMousePos.x = Mathf.Clamp(_virtualMousePos.x, 0f, Screen.width);
        _virtualMousePos.y = Mathf.Clamp(_virtualMousePos.y, 0f, Screen.height);
        InputState.Change(_virtualMouseInput.virtualMouse.position, _virtualMousePos);
    }
}
