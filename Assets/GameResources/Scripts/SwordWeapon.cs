using DG.Tweening;
using Shapes;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class SwordWeapon : MonoBehaviour
{
    public event Action<bool> OnAnimateAttack = delegate { };

    [SerializeField] private ShapeRenderer myShape;  
    [SerializeField] private AttackController myAttackController;
    [SerializeField] private Vector3 range = new Vector3(0f,0f,90f);
    private Quaternion startPos;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float knokback = 20f;
    public Collider2D RangeAttack { get; private set; }
        
    private void Awake()
    {
        RangeAttack = GetComponent<Collider2D>();
    }

    private void Start()
    {
        startPos = transform.localRotation;
        myAttackController.OnShot += AnimateAttack;
    }

    private void AnimateAttack()
    {
        myShape.enabled = true;
        RangeAttack.enabled = true;
        transform.DOLocalRotate(transform.localRotation.eulerAngles + range, speed, RotateMode.FastBeyond360);
        
        OnAnimateAttack?.Invoke(true);
    }

    private void EndAnimateWeapon()
    {
        myShape.enabled = false;
        RangeAttack.enabled = false;
        transform.localRotation = startPos;
        //off animation
        OnAnimateAttack?.Invoke(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            var direction = (enemy.transform.position - transform.position);
            enemy.GetComponent<Rigidbody2D>().AddForce(direction * knokback, ForceMode2D.Impulse);
            enemy.GetDamage(myAttackController.Damage);
        }        
    }

    public void EndAnimate()
    {
        EndAnimateWeapon();
    }

    private void OnDestroy()
    {
        myAttackController.OnShot -= AnimateAttack;
    }
}