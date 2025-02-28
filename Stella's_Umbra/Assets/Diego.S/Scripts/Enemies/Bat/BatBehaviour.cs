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
    [SerializeField] EnemiesAreaBehaviour _enemyAreaBehaviour;
    [HideInInspector] public Vector3 _startingPoint;
    private Transform _player;
    private Animator _anim;
    private SpriteRenderer _renderer;

    private bool isPlayingFlySound = false;
    private bool isPlayingAttackSound = false;
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

        //Si está cerca del jugador, reproducir sonido de vuelo
        if (_distance < 10f && _enemyAreaBehaviour != null && !isPlayingFlySound)
        {
            _enemyAreaBehaviour.FollowingState();

            isPlayingFlySound = true;
            AudioManagerBehaviour.instance.PlaySFX("Bat Flying");
            //Se reproduce cada 1s
            Invoke("ResetFlySound", 1f);
        }
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

    public void Attack()
    {
        if (!isPlayingAttackSound)
        {
            isPlayingAttackSound = true;
            AudioManagerBehaviour.instance.PlaySFX("Bat Attack");
            //Suena cada 0.4s
            Invoke("ResetAttackSound", 0.4f);
        }
    }

    void ResetFlySound()
    {
        isPlayingFlySound = false;
    }

    void ResetAttackSound()
    {
        isPlayingAttackSound = false;
    }
}
