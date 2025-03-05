using UnityEngine;

public class Hp : MonoBehaviour
{      
    
     private float knockbackTimer;
        private Vector3 knockbackDirection;
    public int health = 100;

    public void TakeDamage(int damage)
    {
        Debug.Log(damage);
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

    }


}

