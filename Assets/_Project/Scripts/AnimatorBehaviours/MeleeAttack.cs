using KG.CombatCore;
using KG.Stats;
using UnityEngine;

public class MeleeAttack : StateMachineBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float _startTime = 0f;

    [Range(0, 1)]
    [SerializeField] private float _endTime = 0f;

    [SerializeField] private bool DebugMode = false;

    private Combat _combat;
    private StatsHolder _statsHolder;

    private bool _start = false;
    private bool _end = false;

    public bool rightArm = true;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combat = animator.GetComponent<Combat>();
        _statsHolder = animator.GetComponent<StatsHolder>();
        _start = false;
        _end = false;
        if (Mathf.Approximately(_startTime, 0f))
        {
            _combat.StartDamage(rightArm);
            _start = true;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (DebugMode) Debug.Log(stateInfo.normalizedTime);

        var normTime = stateInfo.normalizedTime;
        if (normTime > _startTime && normTime < _endTime && !_start)
        {
            _combat.StartDamage(rightArm);
            _start = true;
            if (DebugMode) Debug.Log("Start damage");
        }
        else if (normTime > _endTime && !_end)
        {
            _combat.EndDamage(rightArm);
            _end = true;
            if (DebugMode) Debug.Log("End damage");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combat.EndDamage(rightArm);
    }
}
