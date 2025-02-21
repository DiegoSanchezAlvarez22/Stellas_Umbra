using UnityEngine;

public class Flag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Verifica si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            CheckPointSystem checkPointSystem = other.GetComponent<CheckPointSystem>();

            if (checkPointSystem != null) 
            {
                checkPointSystem.SaveProgress();

                Debug.Log("Progreso guardado al entrar en el Checkpoint");
            }
        }
    }
}