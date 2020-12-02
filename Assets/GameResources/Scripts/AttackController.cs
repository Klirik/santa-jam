using System;
using System.Collections;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    public Weapon MyWeapon { get { return weapon; } }
    public event Action OnShot = delegate { };
    
    public float DelayAttack { get; private set; } = 0.5f;
    private float rechargeAttack = 0f;
    
    public void Attack()
    {
        if (rechargeAttack <= 0)
        {
            rechargeAttack = DelayAttack;
            Shot();            
            StartCoroutine(RechargeAttack());
        }
    }

    private IEnumerator RechargeAttack()
    {
        while (rechargeAttack > 0)
        {
            rechargeAttack -= Time.deltaTime;
        }
        yield return null;
    }
            
    private void Shot()
    {
        weapon.RangeAttack.enabled = true;
        OnShot?.Invoke();        
    }
}
