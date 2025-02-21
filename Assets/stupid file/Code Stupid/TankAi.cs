using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : MonoBehaviour
{
    public int Hp;
    public int currentHp;
    public Transform player;
    public GameObject rockPrefab;
    public Transform throwPoint;
    public Animator animator;
    public NavMeshAgent agent;

    public float meleeRange = 3f;
    public float throwRange = 15f;
    public float attackCooldown = 3f;
    public float throwCooldown = 5f;

    private float nextAttackTime = 0f;
    private float nextThrowTime = 0f;

    void Start()
    {
        currentHp = Hp;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        if
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Nếu trong tầm đánh, chọn ngẫu nhiên giữa cận chiến & ném đá
        if (Time.time >= nextAttackTime && distance <= throwRange)
        {
            if (distance <= meleeRange || Random.value > 0.5f) // 50% đánh gần, 50% ném đá
            { 
                agent.SetDestination(player.position);
                MeleeAttack();
            }
            else
            {
                agent.SetDestination(transform.position);
                ThrowRock();
            }

            nextAttackTime = Time.time + attackCooldown;
        }

       
    }

    void MeleeAttack()
    {
        animator.SetTrigger("Melee");
        Debug.Log("Tank performs a melee attack!");
    }

    void ThrowRock()
    {
        if (Time.time < nextThrowTime || rockPrefab == null) return;
        
        animator.SetTrigger("Throw");
        Invoke("SpawnRock", 0.5f);
        nextThrowTime = Time.time + throwCooldown;
    }

   void SpawnRock()
{
    if (rockPrefab == null || throwPoint == null || player == null) return;

    GameObject rock = Instantiate(rockPrefab, throwPoint.position, Quaternion.identity);
    if (rock == null) return;

    Rigidbody rb = rock.GetComponent<Rigidbody>();
    if (rb != null)
    {
        Vector3 direction = (player.position - throwPoint.position).normalized; // Hướng về người chơi
        rb.linearVelocity = direction * 20f; // Tăng tốc độ để ném mạnh hơn

        // Xoay viên đá theo hướng bay
        rock.transform.rotation = Quaternion.LookRotation(direction);

        // Thêm xoay vòng khi bay (giống hiệu ứng ném đá)
        rb.angularVelocity = Random.insideUnitSphere * 10f;
    }
}

    Vector3 CalculateThrowVelocity(Vector3 startPoint, Vector3 targetPoint, float timeToTarget)
    {
        Vector3 velocity = (targetPoint - startPoint) / timeToTarget;
        velocity.y += 0.5f * Mathf.Abs(Physics.gravity.y) * timeToTarget;
        return velocity;
    }

    public void TakeDame(int Dame)
    {
        currentHp -= Dame;
        if (currentHp <= 0)
        {
            
            Die();
        }
    }
    void Die()
    {
        agent.isStopped = true;
        animator.enabled = false;
        Destroy(gameObject,5f);
    }

}
