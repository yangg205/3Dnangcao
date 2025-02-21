using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WaypointZombieAI : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public enum ZombieState { Walk, Chase, Attack, Dead }
    public Animator animator;
    public ZombieState currentState = ZombieState.Walk;
    public Transform player;
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float attackCooldown = 2f;
    public float attackDelay = 1.5f;
    public int damage = 10;
    public int health = 100;
    private CapsuleCollider capsuleCollider;
    private bool isAttacking;
    private bool isMoving = false;
    private float lastAttackTime;
    public GameObject bloodScreenEffect;
    private GameObject instantiatedObject;
    void Start()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        navAgent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown;
    }
    void Update()
    {
        switch(currentState)
        {
            case ZombieState.Walk:
            if(!isMoving || navAgent.remainingDistance < 0.1f)
                {
                    Walk();
                }
            if(IsPlayerInRange(chaseDistance))
                    currentState = ZombieState.Chase;
                    break;
            case ZombieState.Chase:
                ChasePlayer();
                if(IsPlayerInRange(attackDistance))
                    currentState = ZombieState.Attack;
                break;
                case ZombieState.Attack:
                AttackPlayer();
                if (IsPlayerInRange(attackDistance))
                    currentState = ZombieState.Chase;
                break;
            case ZombieState.Dead:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isDead", true);
                navAgent.enabled = false;
                capsuleCollider.enabled = false;
                enabled = false;
                GameManager.instance.currentScore += 1;
                Debug.Log("Dead");
                break;

        }
    }
    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.position) <= range;
    }
    private void Walk()
    {
        navAgent.speed = 0.3f;
        Vector3 randomPosition = RandomNavMeshPosition();
        navAgent.SetDestination(randomPosition);
        isMoving = true;
    }
    private Vector3 RandomNavMeshPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
        return hit.position;
    }
    private void ChasePlayer()
    {
        navAgent.speed = 3f;
        //animation
        animator.SetBool("isChasing", true);
        animator.SetBool("isAttacking", false);
        navAgent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        //animation
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", true);
        navAgent.SetDestination(transform.position);
        if(!isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            StartCoroutine(AttackWithDelay());
            StartCoroutine(ActivateBloodScreenEffect());
            lastAttackTime = Time.time;

            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.TakeDamage(damage);
            }
        }
    }
    private IEnumerator AttackWithDelay()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }
    private IEnumerator ActivateBloodScreenEffect()
    {
        InstantiateObject();
        yield return new WaitForSeconds(attackDelay);
        DeleteObject();
    }
    public void TakeDamage(int damageAmount)
    {
        if (currentState == ZombieState.Dead)
            return;
        health -= damageAmount;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    private void Die()
    {
        currentState = ZombieState.Dead;
    }
    void InstantiateObject()
    {
        instantiatedObject = Instantiate(bloodScreenEffect);
    }
    void DeleteObject()
    {
        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);

            instantiatedObject = null;
        }
    }
}
