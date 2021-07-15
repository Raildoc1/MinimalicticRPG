using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorProxy : MonoBehaviour
{

    public float stopTime = 0.25f;

    #region Public animator variables

    #region INT

    public int currentState
    {
        get
        {
            return animator.GetInteger(currentStateParamID);
        }
        set
        {
            animator.SetInteger(currentStateParamID, value);
        }
    }

    public int Poise
    {
        set
        {
            animator.SetInteger(poiseParamID, value);
        }
    }

    #endregion

    #region FLOAT

    public float inputMagnitude
    {
        get
        {
            return animator.GetFloat(inputMagnitudeParamID);
        }
        set
        {
            animator.SetFloat(inputMagnitudeParamID, value, stopTime, Time.deltaTime);
        }
    }

    public float inputVertical
    {
        get
        {
            return animator.GetFloat(inputVerticalParamID);
        }

        set
        {
            animator.SetFloat(inputVerticalParamID, Mathf.Clamp(value, -1f, 1f), stopTime, Time.deltaTime);
        }
    }

    public float inputHorizontal
    {
        get
        {
            return animator.GetFloat(inputHorizontalParamID);
        }

        set
        {
            animator.SetFloat(inputHorizontalParamID, Mathf.Clamp(value, -1f, 1f), stopTime, Time.deltaTime);
        }
    }

    public float inAirTimer
    {
        get
        {
            return animator.GetFloat(inAirTimerParamID);
        }

        set
        {
            animator.SetFloat(inAirTimerParamID, value);
        }
    }

    #endregion

    #region BOOL

    public bool isStrafing
    {
        get
        {
            return animator.GetBool(isStrafingParamID);
        }
        set
        {
            animator.SetBool(isStrafingParamID, value);
        }
    }

    public bool isDead
    {
        get
        {
            return animator.GetBool(isDeadParamID);
        }
        set
        {
            animator.SetBool(isDeadParamID, value);
        }
    }

    public bool isEquiping
    {
        get
        {
            return animator.GetBool(isEquipingParamID);
        }
        set
        {
            animator.SetBool(isEquipingParamID, value);
        }
    }

    public bool isUnequiping
    {
        get
        {
            return animator.GetBool(isUnequipingParamID);
        }
        set
        {
            animator.SetBool(isUnequipingParamID, value);
        }
    }

    public bool gettingDamage
    {
        get
        {
            return animator.GetBool(gettingDamageParamID);
        }
    }

    public bool inDodge
    {
        get
        {
            return animator.GetBool(inDodgeParamID);
        }
    }

    public bool cannotBlock
    {
        get
        {
            return animator.GetBool(cannotBlockParamID);
        }
    }

    public bool hasSword
    {
        get
        {
            return animator.GetBool(hasSwordParamID);
        }

        set
        {
            animator.SetBool(hasSwordParamID, value);
        }
    }

    public bool lockRotation
    {
        get
        {
            return animator.GetBool(lockRotationParamID);
        }
    }

    public bool inAttack
    {
        get
        {
            return animator.GetBool(inAttackParamID);
        }
    }

    public bool enteringAttack
    {
        get
        {
            return animator.GetBool(enteringAttackParamID);
        }
    }

    public bool startingJump
    {
        get {
            return animator.GetBool(startingJumpParamID);
        }

        set
        {
            animator.SetBool(startingJumpParamID, value);
        }
    }

    public bool isGrounded
    {
        get {
            return animator.GetBool(isGroundedParamID);
        }

        set {
            animator.SetBool(isGroundedParamID, value);
        }
    }

    #endregion

    #region VOID
    public void GetDamage()
    {
        animator.SetTrigger(getDamageParamID);
    }

    public void Equip()
    {
        animator.SetTrigger(equipParamID);
    }

    public void Unequip()
    {
        animator.SetTrigger(unequipParamID);
    }

    public void PickUp()
    {
        animator.SetTrigger(pickUpParamID);
    }

    public void Attack()
    {
        animator.SetTrigger(attackParamID);
    }

    public void Jump()
    {
        animator.SetTrigger(jumpParamID);
    }

    public void StopAction()
    {
        animator.SetTrigger(stopActionParamID);
    }

    public void SetAxisInput(Vector2 input)
    {
        animator.SetFloat(inputHorizontalParamID, Mathf.Clamp(input.x, -1f, 1f), stopTime, Time.deltaTime);
        animator.SetFloat(inputVerticalParamID, Mathf.Clamp(input.y, -1f, 1f), stopTime, Time.deltaTime);
    }

    public void ResetInput(bool immediate = false)
    {
        animator.SetFloat(inputMagnitudeParamID, 0f, immediate ? 0f : stopTime, Time.deltaTime);
        animator.SetFloat(inputVerticalParamID, 0f, immediate ? 0f : stopTime, Time.deltaTime);
        animator.SetFloat(inputHorizontalParamID, 0f, immediate ? 0f : stopTime, Time.deltaTime);
    }

    public void Dodge()
    {
        animator.SetTrigger(dodgeParamID);
    }
    #endregion

    #endregion

    #region Names and IDs

    #region FLOATS

    private readonly string inputMagnitudeParamName = "InputMagnitude";
    private readonly string inputVerticalParamName = "InputVertical";
    private readonly string inputHorizontalParamName = "InputHorizontal";
    private readonly string inAirTimerParamName = "InAirTimer";

    private int inputMagnitudeParamID;
    private int inputVerticalParamID;
    private int inputHorizontalParamID;
    private int inAirTimerParamID;

    #endregion

    #region TRIGGERS

    private readonly string attackParamName = "Attack";
    private readonly string equipParamName = "Equip";
    private readonly string unequipParamName = "Unequip";
    private readonly string getDamageParamName = "GetDamage";
    private readonly string pickUpParamName = "PickUp";
    private readonly string dodgeParamName = "Dodge";
    private readonly string jumpParamName = "Jump";
    private readonly string stopActionParamName = "StopAction";

    private int attackParamID;
    private int equipParamID;
    private int unequipParamID;
    private int getDamageParamID;
    private int pickUpParamID;
    private int dodgeParamID;
    private int jumpParamID;
    private int stopActionParamID;

    #endregion

    #region BOOLS
    
    private readonly string isStrafingParamName = "IsStrafing";
    private readonly string isEquipingParamName = "IsEquiping";
    private readonly string isUnequipingParamName = "IsUnequiping";
    private readonly string isDeadParamName = "IsDead";
    private readonly string inDodgeParamName = "InDodge";
    private readonly string cannotBlockParamName = "CannotBlock";
    private readonly string gettingDamageParamName = "GettingDamage";
    private readonly string hasSwordParamName = "HasSword";
    private readonly string lockRotationParamName = "LockRotation";
    private readonly string inAttackParamName = "InAttack";
    private readonly string enteringAttackParamName = "EnteringAttack";
    private readonly string startingJumpParamName = "StartingJump";
    private readonly string isGroundedParamName = "IsGrounded";
    private readonly string landingParamName = "Landing";

    private int isStrafingParamID;
    private int isEquipingParamID;
    private int isUnequipingParamID;
    private int isDeadParamID;
    private int inDodgeParamID;
    private int cannotBlockParamID;
    private int gettingDamageParamID;
    private int hasSwordParamID;
    private int lockRotationParamID;
    private int inAttackParamID;
    private int enteringAttackParamID;
    private int startingJumpParamID;
    private int isGroundedParamID;
    private int landingParamID;

    #endregion

    #region Integers

    private readonly string currentStateParamName = "CurrentState";
    private readonly string poiseParamName = "Poise";

    private int currentStateParamID;
    private int poiseParamID;

    #endregion

    #endregion

    public bool InAttack => animator.GetCurrentAnimatorStateInfo(2).IsTag("Attack");
    public bool BlockMovement => animator.GetCurrentAnimatorStateInfo(2).IsTag("BlockMovement");
    public bool Landing => animator.GetBool(landingParamID);

    public void GotoState(string stateName, bool isAciton = true)
    {
        animator.ResetTrigger(stopActionParamID);
        animator.Play(stateName);
    }

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        inputMagnitudeParamID = Animator.StringToHash(inputMagnitudeParamName);
        inputVerticalParamID = Animator.StringToHash(inputVerticalParamName);
        inputHorizontalParamID = Animator.StringToHash(inputHorizontalParamName);
        inAirTimerParamID = Animator.StringToHash(inAirTimerParamName);

        attackParamID = Animator.StringToHash(attackParamName);
        equipParamID = Animator.StringToHash(equipParamName);
        unequipParamID = Animator.StringToHash(unequipParamName);
        getDamageParamID = Animator.StringToHash(getDamageParamName);
        pickUpParamID = Animator.StringToHash(pickUpParamName);
        dodgeParamID = Animator.StringToHash(dodgeParamName);
        jumpParamID = Animator.StringToHash(jumpParamName);
        stopActionParamID = Animator.StringToHash(stopActionParamName);

        isStrafingParamID = Animator.StringToHash(isStrafingParamName);
        isEquipingParamID = Animator.StringToHash(isEquipingParamName);
        isUnequipingParamID = Animator.StringToHash(isUnequipingParamName);
        isDeadParamID = Animator.StringToHash(isDeadParamName);
        inDodgeParamID = Animator.StringToHash(inDodgeParamName);
        cannotBlockParamID = Animator.StringToHash(cannotBlockParamName);
        gettingDamageParamID = Animator.StringToHash(gettingDamageParamName);
        hasSwordParamID = Animator.StringToHash(hasSwordParamName);
        lockRotationParamID = Animator.StringToHash(lockRotationParamName);
        inAttackParamID = Animator.StringToHash(inAttackParamName);
        enteringAttackParamID = Animator.StringToHash(enteringAttackParamName);
        startingJumpParamID = Animator.StringToHash(startingJumpParamName);
        isGroundedParamID = Animator.StringToHash(isGroundedParamName);
        landingParamID = Animator.StringToHash(landingParamName);

        currentStateParamID = Animator.StringToHash(currentStateParamName);
        poiseParamID = Animator.StringToHash(poiseParamName);
    }

    public void ApplyRootMotion()
    {
        animator.ApplyBuiltinRootMotion();
    }

}
