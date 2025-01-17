using UnityEngine;

public class Teletransporte : MonoBehaviour
{
    public Transform jugador; // El transform del jugador
    public Transform[] destinos; // Lista de destinos en el mundo

    public void Teletransportar(int indiceDestino)
    {
        if (indiceDestino >= 0 && indiceDestino < destinos.Length)
        {
<<<<<<< HEAD
            jugador.position = destinos[indiceDestino].position;
=======
            jugador.position = new Vector3(destinos[indiceDestino].position.x, destinos[indiceDestino].position.y + 50, destinos[indiceDestino].position.z - 50);
>>>>>>> 46d7437d5c1df3857f54f94aeb64ee4328e722ba
        }
        else
        {
            Debug.LogError("Índice de destino inválido");
        }
    }
}
