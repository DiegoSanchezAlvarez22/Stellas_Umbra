using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    [SerializeField] private GameObject _target;

    private void Start()
    {
        _target = GameObject.Find("Player");
    }

    void Update()
    {
        this.gameObject.transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, _target.transform.position.z - 10);
    }
}
