using UnityEngine;
using UnityEngine.InputSystem;

public class SuperJump : MonoBehaviour
{
    private InputAction jumpAction;
    private Rigidbody playerRigidbody;

    [SerializeField] private float minJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 20f;

    private float holdStartTime;
    private bool isJumping;

    private void Awake()
    {
        // Inicializa la acción de salto y configura los bindings (tecla espacio y botón X del mando)
        jumpAction = new InputAction(type: InputActionType.Button);
        jumpAction.AddBinding("<Keyboard>/space");
        jumpAction.AddBinding("<Gamepad>/buttonSouth");

        // Obtiene la referencia al Rigidbody del jugador
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Suscribe los eventos de Input System
        jumpAction.started += OnJumpStarted; // Cuando se presiona el botón
        jumpAction.canceled += OnJumpCanceled; // Cuando se libera el botón
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        // Desuscribe los eventos
        jumpAction.started -= OnJumpStarted;
        jumpAction.canceled -= OnJumpCanceled;
        jumpAction.Disable();
    }

    private void OnJumpStarted(InputAction.CallbackContext callbackContext)
    {
        // Registra el momento en que se presionó la tecla/botón
        holdStartTime = (float)callbackContext.startTime;
        isJumping = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext callbackContext)
    {
        if (isJumping)
        {
            // Calcula el tiempo que se mantuvo presionado el botón
            float holdDuration = (float)(callbackContext.time - holdStartTime);

            // Calcula la fuerza del salto en función de la duración de la pulsación
            float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, holdDuration);

            // Aplica el salto al jugador
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Debug.Log("Salto realizado con una fuerza de: {jumpForce}");
            isJumping = false;
        }
    }
}

