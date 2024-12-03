using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Jump : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] private float _fuerzaSalto;
    [SerializeField] private Transform _controladorSuelo;
    [SerializeField] private Vector2 _dimensionesCaja;
    [SerializeField] private LayerMask _queEsSuelo;

    private bool _enSuelo;
    private bool _saltar;

    PlayerInput _input;
    InputAction _jump;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();

        _jump = _input.actions["Jump"];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _saltar = true;
        }

        _enSuelo = Physics2D.OverlapBox(_controladorSuelo.position, _dimensionesCaja, 0, _queEsSuelo);
    }

    private void FixedUpdate()
    {
        if (_saltar && _enSuelo)
        {
            Saltar();
        }

        _saltar = false;
    }

    private void Saltar()
    {
        _rb.AddForce(Vector2.up * _fuerzaSalto, ForceMode.Impulse);
        _enSuelo = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_controladorSuelo.position, _dimensionesCaja);
    }
}
