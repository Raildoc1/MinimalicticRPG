using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterState : MonoBehaviour
{

    public enum State
    { 
        Free,
        Combat,
        Dialog
    }

    public State currentState = State.Free;

    private Animator animator;
    private const string strafingAnimatorParamName = "IsStrafing";
    private const string inCombatAnimatorParamName = "InCombat";
    private const string drawSwordAnimatorParamName = "DrawSword";
    private int strafingAnimatorParamId;
    private int drawSwordAnimatorParamId;
    private int inCombatAnimatorParamId;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        strafingAnimatorParamId = Animator.StringToHash(strafingAnimatorParamName);
        drawSwordAnimatorParamId = Animator.StringToHash(drawSwordAnimatorParamName);
        inCombatAnimatorParamId = Animator.StringToHash(inCombatAnimatorParamName);
    }

    public void SetState(State state)
    {
        currentState = state;
        animator.SetBool(inCombatAnimatorParamId, state == State.Combat);
        animator.SetTrigger(state == State.Combat ? "Equip" : "Unequip");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2) && ((currentState == State.Combat) || (currentState == State.Free)))
        {
            SetState(currentState == State.Free ? State.Combat : State.Free);
        }
    }

}
