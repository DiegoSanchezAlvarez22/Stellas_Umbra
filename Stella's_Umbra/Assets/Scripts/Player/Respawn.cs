using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject _respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RespawnPoint")
        {
            _respawnPoint = other.gameObject;
        }

        if (other.gameObject.name == "DownLimit")
        {
            RespawnPosition();
        }
    }

    private void Update()
    {
        
    }

    private void RespawnPosition()
    {
        //transform.position = _respawnPoint.transform.position;
        //vidas al maximo

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        transform.position = _respawnPoint.transform.position;
    }
}
