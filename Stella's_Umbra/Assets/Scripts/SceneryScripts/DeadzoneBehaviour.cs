using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadzoneBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Si el jugador choca con la DeadZone:
        if (other.CompareTag("Player"))
        {
            //Llamas al script de la vida y al del checkpoint
            PlayerLife vidaJugador = other.GetComponent<PlayerLife>();
            CheckPointSystem checkPointSystem = other.GetComponent<CheckPointSystem>();

            //Comprobar si hay un dato guardado
            if (checkPointSystem != null && PlayerPrefs.HasKey("PlayerVida"))
            {
                //Cargar el �ltimo checkpoint
                checkPointSystem.LoadProgress();
                Debug.Log("Cargando el �ltimo checkpoint...");
            }
            else
            {
                Debug.Log("No hay checkpoint guardado. Regresando al men� principal...");
                //Carga la escena del men� principal
                SceneManager.LoadScene("Menu Principal");
            }
        }
    }
}
