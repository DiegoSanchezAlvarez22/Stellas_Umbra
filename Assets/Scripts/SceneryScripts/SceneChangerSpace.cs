using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerSpace : MonoBehaviour
{
    [SerializeField] string _newScene;
    [SerializeField] private Vector3 _newScenePos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(_newScene);
            other.transform.position = _newScenePos;
        }
    }
}
