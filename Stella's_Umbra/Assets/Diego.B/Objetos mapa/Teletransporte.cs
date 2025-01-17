using UnityEngine;

public class Teletransporte : MonoBehaviour
{
    public Transform jugador; // El transform del jugador
    public Transform[] destinos; // Lista de destinos en el mundo

    public void Teletransportar(int indiceDestino)
    {
        if (indiceDestino >= 0 && indiceDestino < destinos.Length)
        {
            jugador.position = destinos[indiceDestino].position;
        }
        else
        {
            Debug.LogError("Índice de destino inválido");
        }
    }
}
