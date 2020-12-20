using System;
using System.Collections;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public event Action<bool> OnAnimateAttack = delegate { };
    public event Action<int> OnGetExperience = delegate { };

    [SerializeField] private SwordWeapon weapon;
    public SwordWeapon MyWeapon { get { return weapon; } }
    public event Action OnShot = delegate { };
    
    public float DelayAttack { get; private set; } = 0.5f;
    private float rechargeAttack = 0f;

    public float Damage { get; private set; } = 10f;

    [SerializeField] private Player player;

    [SerializeField] private float attackDistance = 0.2f;
    [SerializeField] private float throwPower = 2f;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject disc;
    
    private Vector3 weaponPos = Vector3.right; 


    private Quaternion myDirection;

    private void Start()
    {
        player.OnLvlUp += UpParameters;
        //weapon.OnAnimateAttack += AnimatePers;
        OnAnimateAttack += AnimatePers;
    }

    private void UpParameters(int lvl)
    {
        Damage += 10 * lvl;
        throwPower += 0.2f;
    }

    private void OnDestroy()
    {
        player.OnLvlUp -= UpParameters;
        OnAnimateAttack -= AnimatePers;
    }

    public void Attack()
    {
        //weapon.transform.rotation = MovementController.GetDirection();
        
        switch (MovementController.MyDirection)
        {
            case Direction.Left:
                weaponPos = Vector3.left;
                break;
            case Direction.Up:
                weaponPos = Vector3.up;
                break;
            case Direction.Down:
                weaponPos = Vector3.down;
                break;
            default:
                weaponPos = Vector3.right;
                break;
        }
        
        //weapon.transform.localPosition = weaponPos * attackDistance;

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
    
    public void UpdateDamageAttack(float addDamage)
    {
        Damage += addDamage;
    }

    private void Shot()
    {
        var newDisc = Instantiate(disc, transform.position + weaponPos, Quaternion.identity);
        newDisc.GetComponent<Rigidbody2D>().AddForce(weaponPos*throwPower, ForceMode2D.Impulse);
        var discWeapon = newDisc.GetComponent<DiscWeapon>();
        discWeapon.Init(Damage);
        discWeapon.OnKillEnemy += GetExperince;
        discWeapon.OnDie += ClearSub;
        
        AnimateAttack();
        //weapon.RangeAttack.enabled = true;
        //OnShot?.Invoke();        
    }

    private void ClearSub(GameObject gameObject)
    {
        gameObject.GetComponent<DiscWeapon>().OnKillEnemy -= GetExperince;
        gameObject.GetComponent<DiscWeapon>().OnDie -= ClearSub;
    }

    private void GetExperince(int experience)
    {
        OnGetExperience?.Invoke(experience);
    }

    public void EndAnimation()
    {
        //MyWeapon.EndAnimate();
        Debug.Log("EndAnimate");
        EndAnimateWeapon();
    }

    private Coroutine waitAnimationCoroutine;
    private void AnimateAttack()
    {
        OnAnimateAttack?.Invoke(true);

        if(waitAnimationCoroutine != null)
        {
            StopCoroutine(waitAnimationCoroutine);
        }
        waitAnimationCoroutine = StartCoroutine(WaitStopAnimation());
    }

    private IEnumerator WaitStopAnimation()
    {
        yield return new WaitForSeconds(2f);

        OnAnimateAttack?.Invoke(false);
        waitAnimationCoroutine = null;
    }

    private void EndAnimateWeapon()
    {
        OnAnimateAttack?.Invoke(false);
    }
    
    private void AnimatePers(bool state)
    {
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsAttack", state);
        if (waitAnimationCoroutine != null)
        {
            StopCoroutine(waitAnimationCoroutine);
            waitAnimationCoroutine = null;
        }
    }
}
