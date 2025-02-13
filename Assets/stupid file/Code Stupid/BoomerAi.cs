using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using JetBrains.Annotations;

public class BoomerAi : MonoBehaviour
{
    public string name; 
    private float ChaseSpeed= 5f;
    public float Hp = 100f;
    public NavMeshAgent Nav;
    public  enum ZombieState 
    {
        Chase,
        Attack,
        Dead
    }
    public ZombieState currentState = ZombieState.Chase;
    public Animator animator;
    public Transform player;
    public float chaseDistance =10f;
    public float AttackDistance = 2f;
    public float AttackCooldown = 2f;
    public float AttackDelay = 1.5f;
    private bool isAttacking; 
    private float lastAttackTime;
    void Start()
    {
        gameObject.name = "Type:" + name;
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
            case ZombieState.Chase:
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsAttacking", false);
                //Animations
                Nav.SetDestination(player.position);
                if (Vector3.Distance(transform.position, player.position) <= AttackDistance)
                {
                    currentState = ZombieState.Attack;
                }
                break;
                case ZombieState.Attack:
                //Animations
                animator.SetBool("IsAttacking", true);
                Nav.SetDestination(transform.position);
                if (!isAttacking && Time.time - lastAttackTime >= AttackCooldown)
                {   
                    StartCoroutine(AttackWithdelay());
                    Debug.Log("attack player");
                    //
                }
                if (Vector3.Distance(transform.position, player.position) >= AttackDistance)
                {
                    currentState = ZombieState.Chase;
                }

                break;
                case ZombieState.Dead:
                

               
                Debug.Log("Dead");
                break; 
        }
    }
    private IEnumerator AttackWithdelay()
    {
        isAttacking = true;

        yield return new WaitForSeconds(AttackDelay);

        isAttacking =false;
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
    
}
