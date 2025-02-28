using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparofijado : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField] float _speed;
    [SerializeField] Transform padreOriginal;

    private Transform _objetivoPadre; // Referencia al objeto padre de objetoFijador
    private GameObject _fijador;
    private Renderer _fijadorRenderer; // Referencia al Renderer del fijador
    private bool vueltaPadre = false;

    private void Update()
    {
        MovimientoBala();
    }

    // Método para establecer el padre de objetoFijador como el objetivo
    public void SetFijador(Transform fijador)
    {
        if (fijador != null && fijador.parent != null)
        {
            _objetivoPadre = fijador.parent; // Asigna el padre como el objetivo
            _fijadorRenderer = fijador.GetComponent<Renderer>(); // Obtiene el Renderer del fijador
            if (_fijadorRenderer == null)
            {
                Debug.LogWarning("El fijador no tiene un componente Renderer.");
            }
            Debug.Log("Objetivo (padre) asignado: " + _objetivoPadre.name);
        }
    }

    public void VueltaPadre(Transform fijador)
    {
        if (vueltaPadre == true && fijador != null && fijador.parent != null)
        {
            padreOriginal = fijador.parent;
        }
    }

    // Método opcional para cambiar el padre del proyectil en la jerarquía
    public void ChangeParent(Transform newParent)
    {
        transform.SetParent(newParent);
    }

    private void MovimientoBala()
    {
        // Solo mover si el objetivo (padre) está asignado
        if (_objetivoPadre != null && _fijadorRenderer != null && _fijadorRenderer.enabled)
        {
            // Calcula la dirección hacia el objetivo padre
            Vector3 direccion = (_objetivoPadre.position - transform.position).normalized;
            transform.position += direccion * _speed * Time.deltaTime;
        }
        else
        {
            Debug.LogWarning("No se ha asignado un objetivo para el proyectil.");
            Destroy(this.gameObject);
            Debug.Log("Bala destruida");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAir") || other.CompareTag("EnemyFloor"))
        {
            other.GetComponent<EnemyLifes>().DamageRecieved(_damage); //Perdida de vida del enemigo
            vueltaPadre = true;
            Destroy(gameObject);
        }  
    }
}

