using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.Mathematics;

public class BoomerAi : MonoBehaviour
{
    public float knockbackForce = 500f;
    
    public CapsuleCollider collider;
    waveSpaner spawner;
    public GameObject explosionEffectPrefab; 
    public LayerMask LayerPlayer;
    private float ChaseSpeed = 5f;
    public float currentHp = 100f;
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
    public float AttackDistance = 2f;
    public float AttackCooldown = 2f;
    public float AttackDelay = 1.5f;

    public float explosionRadius = 20f;
    public int explosionDamage = 50;

    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        gameObject.name = "BoomerZombie";
        animator = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        switch (currentState)
        {
            case ZombieState.Chase:
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsAttacking", false);

                
                Nav.SetDestination(player.position);

                if (Vector3.Distance(transform.position, player.position) <= AttackDistance)
                {
                    currentState = ZombieState.Attack;
                }
                break;

            case ZombieState.Attack:
               
                Nav.SetDestination(transform.position);
                Explosion();
                break;

            case ZombieState.Dead:
                // Handle explosion
                Collider[] boom = Physics.OverlapSphere(transform.position, explosionRadius, LayerPlayer);
                HandleExplosionEffect();
                if (boom != null && boom.Length > 0)
                {
                    for (int i = 0; i < boom.Length; i++)
                    {
                        
                       
                    }
                }

                Destroy(gameObject);
                Debug.Log("BoomerZombie is Dead");
                break;
        }
    }



    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            currentState = ZombieState.Dead;
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        Gizmos.DrawWireSphere(transform.position, AttackDistance); // Show explosion radius in the editor
    }
    private void HandleExplosionEffect()
    {
        if (explosionEffectPrefab != null)
        {
            // Instantiate the explosion effect at the zombie's position
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            
            Destroy(explosionEffect, 2f);  
        }
    }
    public void setSpawner(waveSpaner _spawner)
    {
        spawner = _spawner;
    }
    public void Explosion()
    {
        HandleExplosionEffect();
         Collider[] boom = Physics.OverlapSphere(transform.position, explosionRadius, LayerPlayer);
        
                if (boom != null && boom.Length > 0)
                {
                    foreach(Collider Playerr in boom )
                    {
                        Hp hp =  Playerr.GetComponent<Hp>();
                        if(hp != null)
                        {
                            hp.TakeDamage(10);
                        }
                        Rigidbody rb = Playerr.GetComponent<Rigidbody>();
                        if(rb != null)
                        {
                             Vector3 knockbackDir = (player.transform.position - transform.position).normalized;
                            rb.AddForce(knockbackDir * knockbackForce);
                        }
                    }
                }
                Destroy(gameObject);
       
    }
}