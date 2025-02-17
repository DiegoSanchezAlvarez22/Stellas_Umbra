using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyLifes))]
public class BatBehaviour : MonoBehaviour
{
    #region Variables

    [SerializeField] private float _distance;
    [HideInInspector] public Vector3 _startingPoint;
    private Transform _player;
    private Animator _anim;
    private SpriteRenderer _renderer;

    #endregion

    void Start()
    {
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();

        _player = GameObject.Find("Player").transform;
        _startingPoint = transform.position;
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
