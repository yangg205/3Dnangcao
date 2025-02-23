using UnityEngine;

public class Hp : MonoBehaviour
{      
    
     private float knockbackTimer;
        private Vector3 knockbackDirection;
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
    }
     public float knockbackForce = 10f; // Lực knockback
    public float knockbackDuration = 0.2f; // Thời gian knockback
private CharacterController characterController;    public Rigidbody rb;
    private bool isKnockedBack = false;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

void Update()
    {
        if (knockbackTimer > 0)
        {
            characterController.Move(knockbackDirection * knockbackForce * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
        }
    }
    public void ApplyKnockback(Vector3 sourcePosition)
    {
        if (!isKnockedBack)
        {
            isKnockedBack = true;

            // Tính hướng knockback (từ nguồn tấn công về phía người chơi)
            Vector3 knockbackDirection = (transform.position - sourcePosition).normalized;
            knockbackDirection.y = 2; // Giữ nhân vật không bị đẩy lên trời

            // Áp dụng lực knockback
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

            // Hủy knockback sau một thời gian
            Invoke(nameof(ResetKnockback), knockbackDuration);
        }
    }

    void ResetKnockback()
    {
        isKnockedBack = false;
    }
}

