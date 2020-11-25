using System;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public event Action OnShot = delegate { };

    [SerializeField] private Weapon myWeapon;
    [SerializeField] private Collider2D rangeAttack;
    [Space]
    [SerializeField] private float delayAttack = 1f;
    private float rechargeAttack = 0f;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (rechargeAttack <= 0)
        {
            rechargeAttack = delayAttack;
            Shot();
        }
        else if (rechargeAttack > 0)
        {
            rechargeAttack -= Time.deltaTime;
        }
    }

    private void Shot()
    {
        ChangeAttackState(true);
        OnShot?.Invoke();        
    }

    public void ChangeAttackState(bool state)
    {
        rangeAttack.enabled = state;
    }
}
