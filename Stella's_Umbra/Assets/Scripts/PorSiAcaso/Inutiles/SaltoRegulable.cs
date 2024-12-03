using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaltoRegulable : MonoBehaviour
{
    public float speed = 5f;
    public float alturaSalto = 10f;
    public int maxSaltos = 1;

    private Rigidbody rb;
    //private Animator ani;
    private Vector3 direccionMovimiento;
    private bool saltando;
    private int numeroSaltos;
    private bool enElSuelo;

    public Transform puntoChequeoSuelo;
    public float distanciaChequeoSuelo = 0.2f;
    public LayerMask capaSuelo;

    private @Input1 input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //ani = GetComponent<Animator>();

        input = new Input1();

        input.Playing.Walk.performed += OnMover;
        input.Playing.Walk.canceled += OnMover;
        input.Playing.Jump.performed += OnSalto;
    }

    private void OnEnable()
    {
        input.Playing.Enable();
    }

    private void OnDisable()
    {
        input.Playing.Disable();
    }

    private void OnMover(InputAction.CallbackContext context)
    {
        // Lee el valor del joystick o teclas de movimiento
        Vector2 input = context.ReadValue<Vector2>();
        direccionMovimiento = new Vector3(input.x, 0, input.y);
    }

    private void OnSalto(InputAction.CallbackContext context)
    {
        if (enElSuelo || numeroSaltos > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, alturaSalto, rb.linearVelocity.z);
            saltando = true;
            numeroSaltos--;
            //ani.SetTrigger("jump"); // Activar animaci�n de salto
        }
    }

    private void Update()
    {
        // Verificar si el personaje est� en el suelo
        enElSuelo = Physics.CheckSphere(puntoChequeoSuelo.position,
            distanciaChequeoSuelo, capaSuelo);
        if (enElSuelo && !saltando)
        {
            numeroSaltos = maxSaltos;
            //ani.SetBool("isGrounded", true);
        }
        else
        {
            //ani.SetBool("isGrounded", false);
        }
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal en 3D
        Vector3 movimiento = direccionMovimiento * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movimiento);

        // Actualiza la animaci�n de correr
        //ani.SetBool("run", direccionMovimiento.magnitude > 0);

        // Controla la rotaci�n del personaje
        if (direccionMovimiento != Vector3.zero)
        {
            Quaternion nuevaRotacion = Quaternion.LookRotation(direccionMovimiento);
            rb.rotation = Quaternion.Slerp(rb.rotation, nuevaRotacion, Time.fixedDeltaTime * 10f);
        }
    }

    private void OnDrawGizmos()
    {
        // Dibuja el �rea de verificaci�n de suelo para que sea visible en el editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(puntoChequeoSuelo.position, distanciaChequeoSuelo);
    }
}
