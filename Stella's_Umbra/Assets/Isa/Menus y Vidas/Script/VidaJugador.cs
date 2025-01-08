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

            SceneManager.LoadScene("Menu Principal");
            Destroy(gameObject);
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
            Debug.Log("Has Morido");
        }
        cambioVida.Invoke(vidaActual);
        Debug.Log("Vida perdida. Vida actual: " + vidaActual);
    }
}