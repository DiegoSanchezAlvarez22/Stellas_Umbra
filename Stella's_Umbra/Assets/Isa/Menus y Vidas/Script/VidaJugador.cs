using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class VidaJugador : MonoBehaviour
{
    [SerializeField] int cristalesRequeridos;    // Cantidad de items necesarios para aumentar vida
    [SerializeField] int regenerarVida;     // heal
    [SerializeField] int cantidadActualCristales;  // Items actuales recogidos
    [SerializeField] int vidaActual;         // Vida inicial del jugador
    [SerializeField] int corazonesMax;         // Vida máxima del jugador

    public UnityEvent<int> cambioVida;
    public UnityEvent<int> sumarCorazon;

    //JULIO Propiedades para conseguir la vida actual y vida máxima para guardar la info
    public int VidaActual => vidaActual;
    public int VidaMaxima => corazonesMax;
    public int CantidadActualCristales => cantidadActualCristales;

    //JULIO Referencia al script de guardado de datos
    [SerializeField] private CheckPointSystem _checkPointSystem;


    private void Start()
    {
        vidaActual = corazonesMax;
        cambioVida.Invoke(vidaActual);
    }

    private void Update()
    {
        if (vidaActual == 0)
        {
            //Respawn _respawn;
            //_respawn = gameObject.GetComponent<Respawn>();
            //_respawn.RespawnPosition();
            //vidaActual = corazonesMax;
            //cambioVida.Invoke(vidaActual);

            //CorazonesUI _corazonesUI;
            //_corazonesUI = gameObject.GetComponent<CorazonesUI>();
            //cambioVida.AddListener(_corazonesUI.CambiarCorazones);
            //sumarCorazon.AddListener(_corazonesUI.SumarCorazones);


            //JULIO Cuando la vida llege a 0, cargar el último progreso guardado
            //Si muere y tiene alguna Key guardada, que cargue la info guardada
            //Si muere y no tiene una Key guardada, que se reinicie la escena
            if (vidaActual == 0)
            {
                if (_checkPointSystem != null)
                {
                    _checkPointSystem.LoadProgress();
                    Debug.Log("El jugador ha muerto. Cargando el último progreso guardado.");
                }
                else
                {
                    Debug.Log("No se asignó el sistema de checkpoints al jugador.");
                }
            }
        }
    }


    // Método para recoger un item
    public void RecogerItem()
    {
        cantidadActualCristales++;
        Debug.Log("Item recogido. Total de items: " + cantidadActualCristales);

        // Verifica si ya tiene suficientes items para aumentar vida
        if (cantidadActualCristales >= cristalesRequeridos)
        {
            corazonesMax = corazonesMax + 1;
            cantidadActualCristales = 0;
            sumarCorazon.Invoke(corazonesMax);
        }
    }

    // Método para aumentar la vida
    public void AumentarVida(int regenerar)
    {
        if (vidaActual < corazonesMax)
        {
            vidaActual += regenerar;
            cambioVida.Invoke(vidaActual);
            Debug.Log("Vida aumentada. Vida actual: " + vidaActual);
        }
    }

    // Método para disminuir la vida
    public void PerderVida(int daño)
    {
        vidaActual -= daño;
        if (vidaActual <= 0)
        {
            vidaActual = 0;
            Debug.Log("Has muerto");
        }
        cambioVida.Invoke(vidaActual);
        Debug.Log("Vida actual: " + vidaActual);
    }

    //JULIO Para poder guardar la info de la vida
    public void SetVidaActual(int nuevaVida)
    {
        vidaActual = nuevaVida;
        Debug.Log("Vida establecida a: " + vidaActual);
        // Invocar evento para actualizar la UI
        cambioVida.Invoke(vidaActual);
    }

    public void SetVidaMaxima(int nuevaVidaMaxima)
    {
        corazonesMax = nuevaVidaMaxima;
        Debug.Log("Vida máxima establecida a: " + corazonesMax);
        // Actualizar UI
        sumarCorazon.Invoke(corazonesMax);
    }

    //JULIO Para poder guardar la info de los cristales
    public void SetCantidadCristales(int nuevaCantidadCristales)
    {
        cantidadActualCristales = nuevaCantidadCristales;
        Debug.Log("Cantidad de cristales establecida a: " + cantidadActualCristales);
    }
}