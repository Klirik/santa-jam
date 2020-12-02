using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(AttackController))]
public class Player : MonoBehaviour
{
    [SerializeField] private MovementController movement;
    [SerializeField] private AttackController attackController;

    private bool isPlayedAnimation = false;

    private void OnValidate()
    {
        if (!movement)
        {
            Debug.LogError("MovementController field is empty " + name);
        }
        if (!attackController)
        {
            Debug.LogError("AttackController field is empty " + name);
        }
    }

    private void Awake()
    {
        attackController.MyWeapon.OnEndAnimateAttack += SetAnimationState;
    }

    private void Update()
    {
        if (!isPlayedAnimation)
        {
            movement.DoStep();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                attackController.Attack();
                SetAnimationState(true);
            }
        }
    }
    public void SetAnimationState(bool state)
    {
        isPlayedAnimation = state;
    }
    
    private void OnDestroy()
    {
        attackController.MyWeapon.OnEndAnimateAttack -= SetAnimationState;
    }
}
