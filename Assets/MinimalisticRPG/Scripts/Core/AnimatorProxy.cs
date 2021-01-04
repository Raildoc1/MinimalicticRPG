using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorProxy : MonoBehaviour
{

    public float stopTime = 0.25f;

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

    private Animator animator;

    #region FLOATS

    private readonly string inputMagnitudeParamName = "InputMagnitude";
    private readonly string inputVerticalParamName = "InputVertical";
    private readonly string inputHorizontalParamName = "InputHorizontal";

    private int inputMagnitudeParamID;
    private int inputVerticalParamID;
    private int inputHorizontalParamID;

    #endregion

    #region TRIGGERS

    private readonly string attackParamName = "Attack";
    private readonly string equipParamName = "Equip";
    private readonly string unequipParamName = "Unequip";
    private readonly string getDamageParamName = "GetDamage";
    private readonly string pickUpParamName = "PickUp";

    private int attackParamID;
    private int equipParamID;
    private int unequipParamID;
    private int getDamageParamID;
    private int pickUpParamID;

    #endregion

    #region BOOLS

    private readonly string isStrafingParamName = "IsStrafing";
    private readonly string isEquipingParamName = "IsEquiping";
    private readonly string isUnequipingParamName = "IsUnequiping";
    private readonly string isDeadParamName = "IsDead";

    private int isStrafingParamID;
    private int isEquipingParamID;
    private int isUnequipingParamID;
    private int isDeadParamID;

    #endregion

    #region Integers

    private readonly string currentStateParamName = "CurrentState";

    private int currentStateParamID;

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();

        inputMagnitudeParamID = Animator.StringToHash(inputMagnitudeParamName);
        inputVerticalParamID = Animator.StringToHash(inputVerticalParamName);
        inputHorizontalParamID = Animator.StringToHash(inputHorizontalParamName);

        attackParamID = Animator.StringToHash(attackParamName);
        equipParamID = Animator.StringToHash(equipParamName);
        unequipParamID = Animator.StringToHash(unequipParamName);
        getDamageParamID = Animator.StringToHash(getDamageParamName);
        pickUpParamID = Animator.StringToHash(pickUpParamName);

        isStrafingParamID = Animator.StringToHash(isStrafingParamName);
        isEquipingParamID = Animator.StringToHash(isEquipingParamName);
        isUnequipingParamID = Animator.StringToHash(isUnequipingParamName);
        isDeadParamID = Animator.StringToHash(isDeadParamName);

        currentStateParamID = Animator.StringToHash(currentStateParamName);
    }

}
