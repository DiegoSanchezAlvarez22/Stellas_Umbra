using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    private GameObject _target;

    private void Start()
    {
        _target = GameObject.Find("Player");
    }

    void Update()
    {
        if (_target == null)
        {
            _target = GameObject.Find("Player");
        }
        gameObject.transform.position = new Vector3
            (_target.transform.position.x,
            _target.transform.position.y + 2.5f,
            _target.transform.position.z - 10f);
    }
}
