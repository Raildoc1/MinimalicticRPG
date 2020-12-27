using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolBehaviour : StateMachineBehaviour {

    [Range(0 ,1)]
    public float startTime = 0f;
    [Range(0 ,1)]
    public float endTime = 1f;
    
    public string boolName;
    public bool startValue;
    public bool endValue;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(startTime < 0.01f) animator.SetBool(boolName, startValue);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var time = stateInfo.normalizedTime;

        animator.SetFloat("FullLayerNormTime", time);

        if (time >= startTime && time < endTime) {
            animator.SetBool(boolName, startValue);
        } else if (time >= endTime) {
            animator.SetBool(boolName, endValue);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool(boolName, endValue);
    }
}
