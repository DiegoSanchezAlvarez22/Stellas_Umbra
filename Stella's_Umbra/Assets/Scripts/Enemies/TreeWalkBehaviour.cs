using UnityEngine;
using System.Collections;

public class TreeWalkBehaviour : MonoBehaviour
{
    private Vector3 lastPosition;
    private bool isPlayingWalkSound = false;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        // Si la posición cambió y el sonido no está reproduciéndose
        if (transform.position != lastPosition && !isPlayingWalkSound)
        {
            isPlayingWalkSound = true;
            AudioManagerBehaviour.instance.PlaySFX("Tree Walking");
            StartCoroutine(ResetWalkSound()); // Inicia un temporizador antes de permitir otro sonido
        }

        lastPosition = transform.position; // Actualiza la posición guardada
    }

    IEnumerator ResetWalkSound()
    {
        yield return new WaitForSeconds(1.5f);
        isPlayingWalkSound = false;
    }
}
