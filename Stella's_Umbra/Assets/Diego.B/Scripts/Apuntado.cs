using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Apuntado : MonoBehaviour
{
    [SerializeField] Transform shootingPoint;
    private Vector3 shootingPointOriginal;
    private BoxCollider boxCollider;
    [SerializeField] float tamañoAgachado;

    [Header("Aataque a Distancia")]
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] Transform fijador;
    private Transform fijador2;
    [SerializeField] float alturaSobrePadre = 1.0f; // Altura deseada encima del nuevo padre
    GameObject rangoDeApuntado;
    private Transform padreOriginal;
    private Vector3 posicionOriginal;
    [SerializeField] GameObject projectile;

    [Header("Ataque Desde Arriba y Especial")]
    [SerializeField] float Energia = 0;
    [SerializeField] GameObject roca;
    [SerializeField] GameObject Tornado;
    [SerializeField] float duracionAtaque;
    [SerializeField] Vector3 newShootingPointDerecha;
    [SerializeField] Vector3 newShootingPointIzquierda;
    private float lastXPosition;     // Variable para almacenar la última posición en X del objeto padre

    void Start()
    {
        fijador2 = fijador.GetChild(0);
        // Guardamos el padre y la posición original del objeto hijo
        posicionOriginal = fijador.localPosition;
        fijador2.GetComponent<Renderer>().enabled = false;
        padreOriginal = fijador.transform.parent;
        shootingPointOriginal = shootingPoint.localPosition;
        // Inicializa la última posición en X del objeto padre
        lastXPosition = transform.localPosition.x;
    }

    //Establecer Fijador
    private void OnTriggerEnter(Collider sphereCollider)
    {
        //Debug.Log(other.gameObject.name);
        if (sphereCollider.CompareTag("Interactuable") || sphereCollider.CompareTag("EnemyAir") || sphereCollider.CompareTag("EnemyFloor"))
        {
            Debug.Log("El objeto ha entrado en el SphereCollider del hijo.");
            fijador.SetParent(sphereCollider.transform);
            Debug.Log("Fijador ahora es hijo de objeto");
            // Coloca el objeto hijo justo encima del nuevo padre
            fijador.localPosition = new Vector3(0, alturaSobrePadre, 0);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactuable") || other.CompareTag("EnemyAir") || other.CompareTag("EnemyFloor"))
        {
            Debug.Log("El objeto ha salido en el SphereCollider del hijo.");
            fijador.transform.parent = padreOriginal;
            fijador.localPosition = new Vector3(0, alturaSobrePadre, 0);
            Debug.Log("Fijador ya no es hijo de objeto");
        }
    }

    private void Update()
    {
        if(Energia <= 100) //100 es el maximo conseguible
        {
            Energia = Energia + Time.deltaTime;
        }
        //Movimiento();
        //DistanceShoot();
        UpShoot();
        StartCoroutine(SpecialAtq());
        Agacharse();
    }

    //Atque distancia
    //void DistanceShoot()
    //{
    //    // Hacer visible el objeto hijo mientras la tecla esté presionada
    //    if (Input.GetKey(KeyCode.Tab))
    //    {
    //        fijador2.GetComponent<Renderer>().enabled = true;
    //    }
    //    else
    //    {
    //        fijador2.GetComponent<Renderer>().enabled = false;
    //    }
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        Debug.Log("Disparando");
    //        GameObject instantiatedBullet;
    //        instantiatedBullet = GameObject.Instantiate(projectile, shootingPoint.position, shootingPoint.rotation);
    //        instantiatedBullet.GetComponent<Disparofijado>().SetFijador(fijador);
    //    }
    //}

    //Atque desde arriba
    void UpShoot()
    {
        // Obtiene la posición actual en X del objeto padre
        float currentXPosition = transform.position.x;

        // Compara la posición actual en X con la última posición en X
        if (currentXPosition >= lastXPosition)
        {
            // Movimiento hacia la derecha
            EjecutarFuncion1();
        }
        else if (currentXPosition <= lastXPosition)
        {
            // Movimiento hacia la izquierda
            EjecutarFuncion2();
        }

        // Actualiza la última posición en X
        lastXPosition = currentXPosition;
    }

    private void EjecutarFuncion1()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            shootingPoint.localPosition = newShootingPointDerecha;
            Debug.Log("Se ha cambiado la posicion de SP");
            Debug.Log("Disparando");
            GameObject instantiatedRoca;
            instantiatedRoca = GameObject.Instantiate(roca, shootingPoint.position, shootingPoint.rotation);
        }
        else
        {
            shootingPoint.localPosition = shootingPointOriginal;
        }
    }

    private void EjecutarFuncion2()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            shootingPoint.localPosition = newShootingPointIzquierda;
            Debug.Log("Se ha cambiado la posicion de SP");
            Debug.Log("Disparando");
            GameObject instantiatedRoca;
            instantiatedRoca = GameObject.Instantiate(roca, shootingPoint.position, shootingPoint.rotation);
        }
        else
        {
            shootingPoint.localPosition = shootingPointOriginal;
        }
    }

    //Ataque especial

    private IEnumerator SpecialAtq()
    {
        if (Input.GetKeyDown(KeyCode.R) && Energia >= 100)
        {
            GameObject instantiatedTornado;
            instantiatedTornado = GameObject.Instantiate(Tornado, new Vector3(transform.localPosition.x, (float)2.5, transform.localPosition.z), shootingPoint.rotation);
            instantiatedTornado.transform.SetParent(this.transform);
            yield return new WaitForSeconds(duracionAtaque);
            Energia = 0;
            Destroy(instantiatedTornado);
        }
        else if (Input.GetKeyDown(KeyCode.R) && Energia < 100)
        {
            Debug.Log("Energia insuficiente");
        }
    }

    private void Agacharse()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * tamañoAgachado, transform.localScale.z);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            boxCollider.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //Atravesar plataformas
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("SueloInestable"))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Bajar la plataforma");
                Physics.IgnoreCollision(collision.collider, boxCollider, true);
            }

        }

    }
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("SueloInestable"))
    //    {
    //        Debug.Log("Reactiva plataforma");
    //        // Reactiva las colisiones con el objeto
    //        Physics.IgnoreCollision(collision.collider, boxCollider, true);
    //    }
    //}
}
