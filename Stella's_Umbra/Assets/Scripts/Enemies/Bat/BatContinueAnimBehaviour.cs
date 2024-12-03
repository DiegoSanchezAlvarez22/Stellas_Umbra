using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatContinueAnimBehaviour : StateMachineBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _baseTime;
    private float _continueTime;
    private Transform _player;
    private BatBehaviour _bat;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _continueTime = _baseTime;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _bat = animator.gameObject.GetComponent<BatBehaviour>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector2.MoveTowards(animator.transform.position,
            _player.position, _moveSpeed * Time.deltaTime);
        _bat.FlipSprite(_player.position);
        _continueTime -= Time.deltaTime;
        if(_continueTime <= 0)
        {
            animator.SetTrigger("Back");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
