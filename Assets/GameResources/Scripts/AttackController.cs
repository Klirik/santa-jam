using System;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    public event Action OnShot = delegate { };
        
    public float DelayAttack { get; private set; } = 0.5f;
    private float rechargeAttack = 0f;

    private bool isAnimateAttack = false;

    private void Awake()
    {
        weapon.OnEndAnimateAttack += EndAnimateAttack;
    }
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
        if (rechargeAttack <= 0 && !isAnimateAttack)
        {
            rechargeAttack = DelayAttack;
            Shot();
            isAnimateAttack = true;
        }
    }

    private void EndAnimateAttack()
    {
        isAnimateAttack = false;
    }
        
    private void Shot()
    {
        weapon.RangeAttack.enabled = true;
        OnShot?.Invoke();        
    }

    private void OnDestroy()
    {
        weapon.OnEndAnimateAttack -= EndAnimateAttack;
    }

}
