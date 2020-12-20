using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(AttackController))]
public class Player : MonoBehaviour
{
    public event Action<int> OnLvlUp = delegate { };
    public event Action OnDie = delegate { };


    [SerializeField] private MovementController movement;
    [SerializeField] private AttackController attackController;

    [SerializeField] private float health = 100f;
    [SerializeField] private float power = 1f;

    [SerializeField] private SpriteRenderer orderInLayer;

    public int Lvl { get; private set; } = 1;
    [SerializeField] private int experience = 0;

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
        attackController.OnAnimateAttack += SetAnimationState;
        attackController.OnGetExperience += GetExperience;
    }

    private void GetExperience(int experience)
    {
        this.experience += experience;
        while(this.experience >= 100*Lvl)
        {
            Lvl++;
            this.experience -= 100;
            OnLvlUp?.Invoke(Lvl);
        }
    }

    private void Update()
    {
        orderInLayer.sortingOrder = -(int)(transform.position.y * 100);
        if (!isPlayedAnimation)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                attackController.Attack();
                SetAnimationState(true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isPlayedAnimation)
        {
            movement.DoStep();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            GetDamage(enemy.Damage);
        }
    }
    public void SetAnimationState(bool state)
    {
        isPlayedAnimation = state;
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        Debug.Log("player health " + health);
    }
        
    private void OnDestroy()
    {
        attackController.OnAnimateAttack -= SetAnimationState;

        attackController.OnGetExperience -= GetExperience;

        OnDie?.Invoke();
    }
}
