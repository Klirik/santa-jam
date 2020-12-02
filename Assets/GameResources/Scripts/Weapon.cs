using DG.Tweening;
using Shapes;
using System;
using System.Collections;
using UnityEngine;

public enum TypeWeapon
{
    Sword,
    Bow
}
//TODO: сделать абстрацию под оружие, наследовать меч и лук от абстракции
[RequireComponent (typeof(Collider2D))]
public class Weapon : MonoBehaviour
{
    public event Action OnEndAnimateAttack = delegate { };

    [SerializeField] private ShapeRenderer myShape;  
    [SerializeField] private AttackController myAttackController;
    [SerializeField] private Vector3 range = new Vector3(0f,0f,90f);
    [SerializeField] Quaternion startPos;
    [SerializeField] private float speed = 1f;

    public Collider2D RangeAttack { get; private set; }

    private Coroutine reseting;

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
        transform.DOLocalRotate(range, speed);
        if (reseting == null)
        {
            reseting = StartCoroutine(ResetWeapon());
        }
    }

    private IEnumerator ResetWeapon()
    {
        yield return new WaitForSeconds(speed);
        myShape.enabled = false;
        transform.localRotation = startPos;
        reseting = null;
        OnEndAnimateAttack?.Invoke();
    }

    private void OnDestroy()
    {
        myAttackController.OnShot -= AnimateAttack;
    }
}