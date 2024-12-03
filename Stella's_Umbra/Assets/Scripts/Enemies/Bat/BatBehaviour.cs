using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _distance;
    public Vector3 _startingPoint;
    private Animator _anim;
    private SpriteRenderer _renderer;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _startingPoint = transform.position;
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _distance = Vector2.Distance(transform.position, _player.position);
        _anim.SetFloat("Distance", _distance);
    }

    public void FlipSprite(Vector3 _target)
    {
        if (transform.position.x < _target.x)
        {
            _renderer.flipX = true;
        }
        else
        {
            _renderer.flipX = false;
        }
    }
}
