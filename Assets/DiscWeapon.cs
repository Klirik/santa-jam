using Shapes;
using System;
using System.Collections;
using UnityEngine;

public class DiscWeapon : MonoBehaviour
{
    public event Action<int> OnKillEnemy = delegate { };
    public event Action<GameObject> OnDie = delegate { };
    private Disc discShape; 

    [SerializeField] private SendCollideEvent targetSeeker;
    [SerializeField] private SendCollideEvent targetFollow;
    [SerializeField] private ParticleSystemRenderer particleSystemRenderer;

    [SerializeField] private float speed = 0.13f;
    private Vector3 direction;

    private Transform target;
    
    public float Damage = 10f;

    [SerializeField] private float knokback = 20f;

    private AttackController attackControllerPlayer;

    public void Init(float damage)
    {
        Damage = damage;
    }

    private void Awake()
    {
        targetSeeker.OnTriggerEnter += SetTarget;
        targetFollow.OnTriggerExit += StopFollowAndBack;
    }
    private void Start()
    {
        discShape = GetComponent<Disc>();
        UpdateSortOrder();
    }
    [ContextMenu("UpdateSortOrder")]
    public void UpdateSortOrder()
    {
        var newOrder = -(int)(transform.position.y * 100);
        discShape.SortingOrder = newOrder;
        particleSystemRenderer.sortingOrder = newOrder;
    }
    private void OnDestroy()
    {
        OnDie?.Invoke(gameObject);
        targetSeeker.OnTriggerEnter -= SetTarget;
        targetFollow.OnTriggerExit -= StopFollowAndBack;

        if(enemy)
            enemy.OnDie -= GetExperience;
    }

    private void Update()
    {
        if (target != null)
        {
            direction = (target.position - transform.position);

            if (direction.magnitude < 0.1)
            {
                direction = Vector3.zero;
                Die();
            }
            transform.Translate(direction.normalized * speed);
        }
        else
        {
            Die();
        }
    }

    public void SetTarget(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() && !target)
        {
            target = collider.transform;
        }
    }

    public void StopFollowAndBack(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>())
        {
            Die();
        }
    }

    private Enemy enemy;
    private int experience = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            var direction = (enemy.transform.position - transform.position);
            enemy.GetComponent<Rigidbody2D>().AddForce(direction * knokback, ForceMode2D.Impulse);
            experience = enemy.Experience;
            enemy.OnDie += GetExperience;
            
            enemy.GetDamage(Damage);
            Die(0.2f);
        }
    }

    private void GetExperience()
    {
        OnKillEnemy?.Invoke(experience);
        enemy.OnDie -= GetExperience;
    }

    public void Die(float sec = 2f)
    {
        StartCoroutine(WaitDie(sec));
    }
    
    private IEnumerator WaitDie(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }
}
