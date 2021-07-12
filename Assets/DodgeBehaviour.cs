using KG.Stats;
using UnityEngine;

public class DodgeBehaviour : StateMachineBehaviour
{
    private StatsHolder _stats;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _stats = animator.GetComponent<StatsHolder>();
        _stats.Dodge();
    }
}
