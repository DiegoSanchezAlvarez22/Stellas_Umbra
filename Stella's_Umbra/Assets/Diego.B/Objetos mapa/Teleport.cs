using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform _player; // El transform del jugador
    public Transform[] _destinies; // Lista de destinos en el mundo

    public void Teleportation(int indiceDestino)
    {
        if (indiceDestino >= 0 && indiceDestino < _destinies.Length)
        {
            _player.position = _destinies[indiceDestino].position;
            _player.position = new Vector3(_destinies[indiceDestino].position.x, _destinies[indiceDestino].position.y, 0);
        }
        else
        {
            Debug.LogError("Índice de destino inválido");
        }
    }
}
