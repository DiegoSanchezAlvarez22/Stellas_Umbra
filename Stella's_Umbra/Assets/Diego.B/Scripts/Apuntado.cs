using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Apuntado : MonoBehaviour
{
    [SerializeField] Transform shootingPoint;
    private Vector3 shootingPointOriginal;
    [SerializeField] float speed = 5f;
    private BoxCollider boxCollider;
    [SerializeField] float tama�oAgachado;
    private Vector3 tama�oJugador;
    [SerializeField] float fuerzaDeSalto = 5f;

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
    [SerializeField] int Energia = 100;
    [SerializeField] GameObject roca;
    [SerializeField] GameObject Tornado;
    [SerializeField] float duracionAtaque;
    [SerializeField] Vector3 newShootingPointDerecha;
    [SerializeField] Vector3 newShootingPointIzquierda;
    private float lastXPosition;     // Variable para almacenar la �ltima posici�n en X del objeto padre

    void Start()
    {
        fijador2 = fijador.GetChild(0);
        //sphereCollider = rangoDeApuntado.GetComponent<SphereCollider>();
        // Guardamos el padre y la posici�n original del objeto hijo
        posicionOriginal = fijador.localPosition;
        fijador2.GetComponent<Renderer>().enabled = false;
        padreOriginal = fijador.transform.parent;
        shootingPointOriginal = shootingPoint.localPosition;
        // Inicializa la �ltima posici�n en X del objeto padre
        lastXPosition = transform.localPosition.x;
        //tama�oJugador = boxCollider.transform.localScale;
    }

    //Establecer Fijador
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.CompareTag("Interactuable"))
        {
            Debug.Log("El objeto ha entrado en el SphereCollider del hijo.");
            fijador.SetParent(other.transform);
            Debug.Log("Fijador ahora es hijo de objeto");
            // Coloca el objeto hijo justo encima del nuevo padre
            fijador.localPosition = new Vector3(0, alturaSobrePadre, 0);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactuable"))
        {
            Debug.Log("El objeto ha salido en el SphereCollider del hijo.");
            fijador.transform.parent = padreOriginal;
            fijador.localPosition = new Vector3(0, alturaSobrePadre, 0);
            Debug.Log("Fijador ya no es hijo de objeto");
        }
    }

    private void Update()
    {
        // Hacer visible el objeto hijo mientras la tecla est� presionada
        if (Input.GetKey(KeyCode.Tab))
        {
            fijador2.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            fijador2.GetComponent<Renderer>().enabled = false;
        }

        Movimiento();
        DistanceShoot();
        UpShoot();
        StartCoroutine(SpecialAtq());
        Agacharse();
    }

    //Atque distancia
    void DistanceShoot()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Disparando");
            GameObject instantiatedBullet;
            instantiatedBullet = GameObject.Instantiate(projectile, shootingPoint.position, shootingPoint.rotation);
            instantiatedBullet.GetComponent<Disparofijado>().SetFijador(fijador2);
        }
    }

    //Atque desde arriba
    void UpShoot()
    {
        // Obtiene la posici�n actual en X del objeto padre
        float currentXPosition = transform.position.x;

        // Compara la posici�n actual en X con la �ltima posici�n en X
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

        // Actualiza la �ltima posici�n en X
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
        if (Input.GetKeyDown(KeyCode.R) && Energia == 100)
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

    //Movimiento
    private void Movimiento()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + 1 * speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - 1 * speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + fuerzaDeSalto * Time.deltaTime, transform.position.z);
        }
    }

    private void Agacharse()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * tama�oAgachado, transform.localScale.z);
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
