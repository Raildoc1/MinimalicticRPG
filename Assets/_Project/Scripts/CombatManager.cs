using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterState))]
public class CombatManager : MonoBehaviour
{

    private Animator animator;
    private CharacterState characterState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterState = GetComponent<CharacterState>();
    }

    void Update()
    {
        if (characterState.currentState != CharacterState.State.Combat)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack");
        }
    }
}
