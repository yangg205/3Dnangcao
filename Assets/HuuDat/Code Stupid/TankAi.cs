using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System;
using Unity.VisualScripting;
using CodeMonkey.HealthSystemCM;
using UnityEngine.InputSystem;
using System.Collections;


public class TankAI : MonoBehaviour
{
    public AudioClip[] SoundDeath;
    public AudioClip[] Soundattack;
    
    public AudioClip[] Soundhurt;
    public AudioSource audioSource;
    public GameObject hitbox;
    public float DissolveRate = 0.00125f;
    public float refreshRate = 0.0025f;
    public Collider collider;
    waveSpaner spawner;
    public float TimeRadollDie;
    [SerializeField, Range(0f, 15f)]
    private float ChaseSpeed = 5f;
    public float MaxHp = 500f;
    [SerializeField]
    private float currentHp;
    public NavMeshAgent Nav;
    public enum ZombieState
    {
        Chase,
        Attack,
        Dead
    }
    public ZombieState currentState = ZombieState.Chase;
    public Animator animator;
    public Transform player;
    public float chaseDistance = 10f;
    
    public float attackRange = 3f;      
    public float attackForce = 500f;
    public float AttackCooldown = 2f;
    public float AttackDelay = 1.5f;
    private bool isAttacking;
    private float lastAttackTime;
    void Start()
    {   
        audioSource = GetComponent<AudioSource>();
        collider = GetComponentInChildren<CapsuleCollider>();
        currentHp = MaxHp;
        gameObject.name = "Type:" + name;
        animator = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
        lastAttackTime = -AttackCooldown;
        foreach (Collider Co in gameObject.GetComponentsInChildren<Collider>())
        {
            Co.enabled = true;

        }
    }
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case ZombieState.Chase:
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsAttacking", false);
                //Animations
                Nav.SetDestination(player.position);
                if (Vector3.Distance(transform.position, player.position) <= attackRange)
                {
                    currentState = ZombieState.Attack;
                }
                break;
            case ZombieState.Attack:
                //Animations
                
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsAttacking", true);
                Nav.SetDestination(player.position);
              //  transform.LookAt(player);
                if (!isAttacking && Time.time - lastAttackTime >= AttackCooldown)
                {
                    StartCoroutine(AttackWithdelay());
                    Debug.Log("attack player");
                    //
                }
                if (Vector3.Distance(transform.position, player.position) >= attackRange)
                {
                    currentState = ZombieState.Chase;
                }

                break;
            case ZombieState.Dead:
            Nav.SetDestination(transform.position);
                StartCoroutine(RadollDie());
             
              
                break;
        }
    }
    private IEnumerator AttackWithdelay()
    {
        isAttacking = true;

        yield return new WaitForSeconds(AttackDelay);

        isAttacking = false;
        lastAttackTime = Time.time;
    }

    public void TakeDamageAmount(int damageAmount)
    {

        if (currentState == ZombieState.Dead)
        {
            
            Debug.Log("die");
            return;
        }
        if(UnityEngine.Random.Range(0, 100) > 50)
        {
        audioSource.PlayOneShot(Soundhurt[UnityEngine.Random.Range(0, Soundhurt.Length)]);
        }
        currentHp -= damageAmount;
        Debug.Log(currentHp);
        if (currentHp <= 0)
        {
            currentHp = 0;
            currentState = ZombieState.Dead;
            audioSource.PlayOneShot(SoundDeath[UnityEngine.Random.Range(0, SoundDeath.Length)]);
        }
    }
    private IEnumerator RadollDie()
    {

        
        animator.enabled = false;
        collider.enabled = false;
        foreach (Collider Co in gameObject.GetComponentsInChildren<Collider>())
        {
            Co.enabled = true;

        }
        foreach (Rigidbody Rig in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            Rig.isKinematic = false;
        }
        yield return new WaitForSeconds(TimeRadollDie);
        foreach (Rigidbody Rig in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            Rig.isKinematic = true;
        }
        foreach (Collider Co in gameObject.GetComponentsInChildren<Collider>())
        {
            Co.enabled = false;

        }
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void setSpawner(waveSpaner _spawner)
    {
        spawner = _spawner;
    }
    public void SetAtive()
    {
        audioSource.PlayOneShot(Soundattack[UnityEngine.Random.Range(0, Soundattack.Length)]);
        hitbox.SetActive(true);
    }
    public void NoAtive()
    {
        hitbox.SetActive(false);
    }
    
}

