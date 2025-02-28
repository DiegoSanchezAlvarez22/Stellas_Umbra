using UnityEngine;
using System.Collections;
using static EnemiesAreaBehaviour;

public class TreeWalkBehaviour : MonoBehaviour
{
    private Vector3 lastPosition;
    private bool isPlayingWalkSound = false;
    private GameObject _playerTarget;

    void Start()
    {
        lastPosition = transform.position;
        _playerTarget = GameObject.FindGameObjectWithTag("Player");
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

        if (Vector2.Distance(transform.position, _playerTarget.transform.position) > 9)
        {
            StopCoroutine(ResetWalkSound());
        }

        lastPosition = transform.position; // Actualiza la posición guardada
    }

    IEnumerator ResetWalkSound()
    {
        yield return new WaitForSeconds(1.5f);
        isPlayingWalkSound = false;
    }
}
