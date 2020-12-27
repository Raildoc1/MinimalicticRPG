using UnityEngine;

public class IntegerChanger : StateMachineBehaviour {

    [Range(0, 1)]
    public float timeThanChange = .5f;

    public string inName;
    public int value;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var time = stateInfo.normalizedTime;

        if (time > timeThanChange) {
            animator.SetInteger(inName, value);
        }
    }
}
