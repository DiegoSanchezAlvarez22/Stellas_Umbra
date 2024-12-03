using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashForce = 20f; // Fuerza del dash
    public float dashDuration = 0.2f; // Duraci�n del dash
    public float dashCooldown = 1f; // Tiempo de espera entre dashes

    private Rigidbody rb;
    private Vector3 dashDirection;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float lastDashTime = -Mathf.Infinity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isDashing)
        {
            dashTime += Time.deltaTime;
            if (dashTime >= dashDuration)
            {
                EndDash();
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        dashDirection = new Vector3(Input.GetAxis("Horizontal"),
            0, Input.GetAxis("Vertical")).normalized;
        if (dashDirection == Vector3.zero) dashDirection = transform.forward;

        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
        isDashing = true;
        dashTime = 0f;
        lastDashTime = Time.time;
    }

    private void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector3.zero; // Detiene el movimiento al finalizar el dash
    }
}

