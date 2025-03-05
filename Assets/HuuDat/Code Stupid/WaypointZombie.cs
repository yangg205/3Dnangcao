using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using JetBrains.Annotations;
using TMPro;
public class WaypointZombie : MonoBehaviour
{
    public float Hp = 100;
    public NavMeshAgent Nav;
    public float ChaseSpeed =1; 
    public enum ZombieState
    {
        Walk,
        Chase,
        Attack,
        Dead
    }
    public ZombieState currentState = ZombieState.Walk;
    public Animator animator;
    public Transform player;
    public float chaseDistance = 10f;
    public float AttackDistance = 2f;
    public float AttackCooldown = 2f;
    public float AttackDelay = 1.5f;
    private bool IsMoving; 

    private bool isAttacking;
    private float lastAttackTime;
    void Start()
    {
        animator = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
        lastAttackTime = -AttackCooldown;
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
            case ZombieState.Walk:
                //Animation 
               if(!IsMoving|| Nav.remainingDistance< 0.1f)
                {
                    Walk();
                }
               if(IsPlayerInRange(chaseDistance))
                {
                    currentState = ZombieState.Chase;
                }
                break;
            case ZombieState.Chase:

                //Animations
                ChasePlayer();
                if (IsPlayerInRange(AttackDistance))
                {
                    currentState = ZombieState.Attack;
                }
                break;
            case ZombieState.Attack:
                //Animations
               
                Nav.SetDestination(transform.position);
                if (!IsPlayerInRange(AttackDistance))
                {

                  AttackPLayer();
                  
                    //
                }
                if (IsPlayerInRange(AttackDistance))
                {
                    currentState = ZombieState.Chase;
                }

                break;
            case ZombieState.Dead:
                
                Nav.enabled = false;
                animator.enabled = false;
                Destroy(gameObject, 2f);
                Debug.Log("Dead");
                break;
        }
    }
    public void ChasePlayer()
    {
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsAttacking",false);
        Nav.speed = 2f;
        Nav.SetDestination(player.position);
    }
    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.position) <= range;
    }
    private void Walk()
    {
        animator.SetBool("IsWalking", false);
        Nav.speed = ChaseSpeed;
        Vector3 randomPosition = RandomNavMeshPosion();
        Nav.SetDestination(randomPosition);
        IsMoving = true;
    }
    private Vector3 RandomNavMeshPosion()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
        return hit.position;
    }
    private IEnumerator AttackWithdelay()
    {
        isAttacking = true;

        yield return new WaitForSeconds(AttackDelay);

        isAttacking = false;
        lastAttackTime = Time.time;
    }
    void TakeDame(int dame)
    {
        Hp -= dame;
        if (Hp < 0)
        {
            Hp = 0;
            currentState = ZombieState.Dead;

        }
    }
    public void AttackPLayer()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", true);
        StartCoroutine(AttackWithdelay());
        Debug.Log("attack player");
    }
}

