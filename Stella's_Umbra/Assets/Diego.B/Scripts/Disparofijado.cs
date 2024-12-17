using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparofijado : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float speed;

    private Transform objetivoPadre; // Referencia al objeto padre de objetoFijador
    private GameObject fijador;
    private Renderer fijadorRenderer; // Referencia al Renderer del fijador

    private void Update()
    {
        MovimientoBala();
    }

    // Método para establecer el padre de objetoFijador como el objetivo
    public void SetFijador(Transform fijador)
    {
        if (fijador != null && fijador.parent != null)
        {
            objetivoPadre = fijador.parent; // Asigna el padre como el objetivo
            fijadorRenderer = fijador.GetComponent<Renderer>(); // Obtiene el Renderer del fijador
            if (fijadorRenderer == null)
            {
                Debug.LogWarning("El fijador no tiene un componente Renderer.");
            }
            Debug.Log("Objetivo (padre) asignado: " + objetivoPadre.name);
        }
        else
        {
            Debug.LogWarning("El objetoFijador no tiene un padre.");
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
        if (objetivoPadre != null && fijadorRenderer != null && fijadorRenderer.enabled)
        {
            // Calcula la dirección hacia el objetivo padre
            Vector3 direccion = (objetivoPadre.position - transform.position).normalized;
            transform.position += direccion * speed * Time.deltaTime;
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
            Destroy(this.gameObject);
            Destroy(other.gameObject);
            Debug.Log("Bala destruida");
        }  
    }
}

