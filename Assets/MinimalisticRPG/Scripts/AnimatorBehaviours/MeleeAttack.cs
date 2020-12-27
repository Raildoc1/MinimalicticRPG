using System.Collections;
using System.Collections.Generic;
using KG.CombatCore;
using UnityEngine;

public class MeleeAttack : StateMachineBehaviour {

    [Range(0, 1)]
    public float start_time = 0f;

    [Range(0, 1)]
    public float end_time = 0f;

    private Combat _combat;

    private bool _start = false;
    private bool _end = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _combat = animator.GetComponent<Combat>();
        _start = false;
        _end = false;
        if (Mathf.Approximately(start_time, 0f)) { 
            _combat.StartDamage();
            _start = true;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var normTime = stateInfo.normalizedTime;
        if (normTime > start_time && normTime < end_time && !_start) {
            _combat.StartDamage();
            _start = true;
        } else if (normTime > end_time && !_end) { 
            _combat.EndDamage();
            _end = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(Mathf.Approximately(end_time, 1f)) _combat.EndDamage();
    }

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
