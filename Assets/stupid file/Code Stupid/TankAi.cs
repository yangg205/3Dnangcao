using UnityEngine;

public class BossAI : MonoBehaviour
{
     private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    public Transform player; // Reference to the player
    public float moveSpeed = 3f; // Movement speed
    public float attackRange = 5f; // Range for attacking
    public float skillCooldown = 3f; // Cooldown for skills
    private float lastSkillTime = 0f; // Last time a skill was used
    public float jumpForce = 5f; // Force applied for the jump
    private Rigidbody rb; // Rigidbody component

    void Update()
    {
      
            // Move towards the player
            MoveTowardsPlayer();

            // Check if the boss can use a skill
            if (Time.time >= lastSkillTime + skillCooldown)
            {
                UseSkill();
                lastSkillTime = Time.time; // Reset the cooldown
            }
        
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void UseSkill()
    {
        int skillIndex = Random.Range(0, 3); // Randomly select a skill

        switch (skillIndex)
        {
            case 0:
                Skill1();
                break;
            case 1:
                Skill2();
                break;
            case 2:
                Skill3();
                break;
        }
    }

    private void Skill1()
    {
        JumpAttack();
    GameObject fireballPrefab = Resources.Load<GameObject>("FireballPrefab");
    GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
    Vector3 direction = (player.position - transform.position).normalized;
    fireball.GetComponent<Rigidbody>().linearVelocity = direction * 10f; // Set speed
    }

    private void Skill2()
    {
        JumpAttack();
        Debug.Log("Boss uses Skill 2: Area Attack!");
        // Implement skill logic here (e.g., deal damage in a radius)
    }

    private void Skill3()
    {
        Debug.Log("Jump");
        // Implement skill logic here (e.g., spawn minions)
        JumpAttack();
    }
    private void JumpAttack()
    {
        // Calculate the jump direction
        Vector3 jumpDirection = (player.position - transform.position).normalized;
        jumpDirection.y = 1; // Set the jump height

        // Apply the jump force
        rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        
        // Optionally, you can add logic to deal damage upon landing
        // Use OnCollisionEnter or similar to check for landing and damage
    }
}