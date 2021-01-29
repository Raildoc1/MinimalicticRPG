using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolBehaviour : StateMachineBehaviour
{

    [System.Serializable]
    public class SetBoolEntity
    {
        [Range(0, 1)]
        public float startTime = 0f;
        [Range(0, 1)]
        public float endTime = 1f;
        public string boolName;
        public bool startValue;
        public bool endValue;
    }

    public List<SetBoolEntity> boolEntities;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        foreach (var b in boolEntities)
        {
            if (b.startTime < 0.01f)
            {
                animator.SetBool(Animator.StringToHash(b.boolName), b.startValue);
            }
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var time = stateInfo.normalizedTime;

        animator.SetFloat("FullLayerNormTime", time);

        foreach (var b in boolEntities)
        {
            if (time >= b.startTime && time < b.endTime)
            {
                animator.SetBool(Animator.StringToHash(b.boolName), b.startValue);
            }
            else if (time >= b.endTime)
            {
                animator.SetBool(Animator.StringToHash(b.boolName), b.endValue);
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var b in boolEntities)
        {
            animator.SetBool(Animator.StringToHash(b.boolName), b.endValue);
        }
    }
}
