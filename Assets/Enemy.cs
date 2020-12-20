using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private event Action OnSetTarget = delegate { };
    public event Action OnDie = delegate { };

    [SerializeField] private SendCollideEvent targetSeeker;
    [SerializeField] private SendCollideEvent targetFollow;
    [SerializeField] private float speed = 0.13f;
    [SerializeField] private GameObject text;
    [SerializeField] private ParticleSystem particleSystem;

    [SerializeField] private ParticleSystemRenderer particleSystemRenderer;
    [SerializeField] private Color friendlyColor;

    private Vector3 direction;
    private AudioSource audioSource;

    [SerializeField] private float health = 100f;
    public float Health
    {
        get
        {
            return health;
        }
        private set
        {
            health = value;
        }
    }

    [SerializeField] private int experience = 10; 
    public int Experience
    {
        get
        {
            return experience;
        }

        private set
        {
            experience = value;
        }
    }

    public float Damage { get; private set; } = 5f;

    private Transform target;

    private GameObject myStartPoint;
    private bool goHome = false;

    [SerializeField] private Boss boss;

    [SerializeField] private bool isAngry = true;

    private void Awake()
    {
        if (boss)
        {            
            boss.OnDie += SetFriendly;
        }
        audioSource = GetComponent<AudioSource>();
        myStartPoint = new GameObject();
        myStartPoint.transform.position = transform.position;
    }

    public void SetFriendly()
    {
        isAngry = false;
        particleSystem.startColor = friendlyColor;
    }

    private void Start()
    {
        targetSeeker.OnTriggerEnter += SetTarget;
        targetFollow.OnTriggerExit += StopFollowAndBack;
    }
       
    private void Update()
    {

        particleSystemRenderer.sortingOrder = -(int)(transform.position.y * 100);

        if (target!= null && isAngry)
        {   
            direction = (target.position - transform.position);
            
            if(direction.magnitude < 0.1)
            {
                direction = Vector3.zero;
                if (goHome)
                {
                    goHome = false;
                    target = null;
                }
            }
            transform.Translate(direction.normalized * speed);
        }
    }       

    public void SetTarget(Collider2D collider)
    {
        if (collider.GetComponent<Player>())
        {
            target = collider.transform;
            if (text)
            {
                text.SetActive(true);                
            }
        }
    }

    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(10f);
        text.SetActive(false);
    }
    
    public void StopFollowAndBack(Collider2D collider)
    {
        if (collider.GetComponent<Player>())
        {
            target = myStartPoint.transform;
            goHome = true;            
        }
    }

    public void UpdateDamageAttack(float addDamage)
    {
        Damage += addDamage;
    }

    public void GetDamage(float damage)
    {
        Debug.Log(damage);
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        audioSource.Play();
    }

    private void OnDestroy()
    {        
        OnDie?.Invoke();
        Destroy(myStartPoint);
        targetSeeker.OnTriggerEnter -= SetTarget;
        targetFollow.OnTriggerExit -= StopFollowAndBack;
        if(boss)
            boss.OnDie -= SetFriendly;
    }

}
