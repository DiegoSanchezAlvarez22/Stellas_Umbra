using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [System.Obsolete]
    void Start()
    {
        CheckPointSystem checkPointSystem = FindObjectOfType<CheckPointSystem>();

        if (checkPointSystem != null)
        {
            if (PlayerPrefs.GetInt("NewGame", 0) == 1)
            {
                checkPointSystem.ClearProgress();
                Debug.Log("Nueva partida iniciada, progreso borrado.");
            }
            else if (PlayerPrefs.GetInt("LoadGame", 0) == 1)
            {
                checkPointSystem.LoadProgress();
                Debug.Log("Partida cargada correctamente.");
            }
        }
    }
}
