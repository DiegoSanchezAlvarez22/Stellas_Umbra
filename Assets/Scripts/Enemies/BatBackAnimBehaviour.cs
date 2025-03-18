using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBackAnimBehaviour : StateMachineBehaviour
{
    [SerializeField] private float _moveSpeed;
    private Vector3 _startingPointAnim;
    private BatBehaviour _bat;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat = animator.gameObject.GetComponent<BatBehaviour>();
        _startingPointAnim = _bat._startingPointBat;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector2.MoveTowards(animator.transform.position,
            _startingPointAnim, _moveSpeed * Time.deltaTime);

        _bat.FlipSprite(_startingPointAnim);

        Debug.Log("animator.transform.position = " + animator.transform.position);
        Debug.Log("_startingPointAnim = " + _startingPointAnim);

        if (Vector3.Distance(animator.transform.position, _startingPointAnim) < 0.3f)
        {
            animator.SetTrigger("StartingPoint");
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
