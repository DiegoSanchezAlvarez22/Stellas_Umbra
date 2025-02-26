using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseUI : MonoBehaviour
{
    private VirtualMouseInput _virtualMouseInput;
    [SerializeField] private RectTransform _canvasRectTransform;
    [SerializeField] private RectTransform _cursorImage; // Imagen del cursor (MouseVirtual)
    private Vector2 _lastMousePosition;
    private bool _usingMouse = false; // Para alternar entre ratón y gamepad

    private void Awake()
    {
        _virtualMouseInput = GetComponent<VirtualMouseInput>();
        Cursor.visible = false; // Ocultar el cursor del sistema
    }

    private void Update()
    {
        transform.localScale = Vector3.one * (1f / _canvasRectTransform.localScale.x);
        transform.SetAsLastSibling();

        // Obtener la posición actual del Virtual Mouse
        Vector2 virtualMousePos = _virtualMouseInput.virtualMouse.position.value;

        // Detectar movimiento del ratón
        Vector2 currentMousePosition = Mouse.current.position.ReadValue();
        if (currentMousePosition != _lastMousePosition)
        {
            _usingMouse = true;
            virtualMousePos = currentMousePosition; // El virtual mouse sigue al ratón
        }
        else if (Gamepad.current != null && Gamepad.current.leftStick.ReadValue() != Vector2.zero)
        {
            _usingMouse = false;
        }

        // Aplicar la posición corregida al Input System y al cursor UI
        InputState.Change(_virtualMouseInput.virtualMouse.position, virtualMousePos);
        _cursorImage.position = virtualMousePos; // Sincronizar la imagen del cursor

        _lastMousePosition = currentMousePosition;
    }

    private void LateUpdate()
    {
        Vector2 _virtualMousePos = _virtualMouseInput.virtualMouse.position.value;
        _virtualMousePos.x = Mathf.Clamp(_virtualMousePos.x, 0f, Screen.width);
        _virtualMousePos.y = Mathf.Clamp(_virtualMousePos.y, 0f, Screen.height);
        InputState.Change(_virtualMouseInput.virtualMouse.position, _virtualMousePos);
        _cursorImage.position = _virtualMousePos; // Asegurar que la imagen del cursor se actualice
    }
}
