using System;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public event Action OnShot = delegate { };

    [SerializeField] private Collider2D rangeAttack;
    
    public float DelayAttack { get; private set; } = 1f;
    private float rechargeAttack = 0f;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        if (rechargeAttack > 0)
        {
            rechargeAttack -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        if (rechargeAttack <= 0)
        {
            rechargeAttack = DelayAttack;
            Shot();
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
