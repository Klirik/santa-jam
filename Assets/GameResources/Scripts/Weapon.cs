using DG.Tweening;
using Shapes;
using System.Collections;
using UnityEngine;

public enum TypeWeapon
{
    Sword,
    Bow
}
//TODO: сделать абстрацию под оружие, наследовать меч и лук от абстракции
public class Weapon : MonoBehaviour
{
    [SerializeField] private ShapeRenderer myShape;  
    [SerializeField] private AttackController myAttackController;
    [SerializeField] private Vector3 range = new Vector3(0f,0f,90f);
    [SerializeField] Quaternion startPos;
    [SerializeField] private float speed = 1f;

    private Coroutine reseting;
    private void Start()
    {
        startPos = transform.localRotation;
        myAttackController.OnShot += AnimateAttack;

        if(speed < myAttackController.DelayAttack)
        {
            speed = myAttackController.DelayAttack;
        }
    }

    private void AnimateAttack()
    {
        myShape.enabled = true;
        transform.DOLocalRotate(range, speed);
        if (reseting == null)
        {
            reseting = StartCoroutine(ResetWeapon());
        }
        myAttackController.ChangeAttackState(false);
    }

    private IEnumerator ResetWeapon()
    {
        yield return new WaitForSeconds(speed);
        myShape.enabled = false;
        transform.localRotation = startPos;
        reseting = null;
    }

    private void OnDestroy()
    {
        myAttackController.OnShot -= AnimateAttack;
    }
}