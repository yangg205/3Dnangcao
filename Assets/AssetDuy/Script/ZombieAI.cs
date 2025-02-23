using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public enum ZombieState { Idle, Chase, Attack, Dead}
    public Animator animator;
    public ZombieState currentState = ZombieState.Idle;
    public Transform player;
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float attackCooldown = 4f;
    public float attackDelay = 3.5f;
    public int damage = 10;
    public int health = 100;
    private CapsuleCollider capsuleCollider;
    private bool isAttacking;
    private float lastAttackTime;
    public GameObject bloodScreenEffect;
    private GameObject instantiatedObject;
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.Log("Player object not found");
        }
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        navAgent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown;
    }
    void Update()
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", false);
                if (Vector3.Distance(transform.position, player.position) <= chaseDistance)
                    currentState = ZombieState.Chase; 
                break;
            case ZombieState.Chase:
                animator.SetBool("isRunning", true);
                animator.SetBool("isAttacking", false);
                navAgent.SetDestination(player.position);
                if (Vector3.Distance(transform.position, player.position) <= attackDistance)
                    currentState = ZombieState.Attack;
                break;
            case ZombieState.Attack:
                animator.SetBool("isAttacking", true);
                navAgent.SetDestination(transform.position);
                if(!isAttacking && Time.time - lastAttackTime >= attackDelay)
                {
                    StartCoroutine(AttackWithDelay());
                    Debug.Log("Attack Player");
                    StartCoroutine(ActivateBloodScreenEffect());
                }
                if (Vector3.Distance(transform.position, player.position) > attackDistance)
                    currentState = ZombieState.Chase;
                break;
            case ZombieState.Dead:
                animator.SetBool("isRunning", false);
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
    private IEnumerator AttackWithDelay()
    {
        isAttacking = true;
        //damage the player
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if(playerMovement != null)
        {
            playerMovement.TakeDamage(damage);
        }
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        lastAttackTime = Time.time;
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
        if(health <= 0)
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
        if(instantiatedObject != null)
        {
            Destroy(instantiatedObject);

            instantiatedObject = null;
        }
    }
}
