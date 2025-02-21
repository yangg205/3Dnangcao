using UnityEngine;

public class TankAttack : MonoBehaviour
{
    
    public float knockbackForce = 500f; // Lực đẩy khi trúng đòn

    private void Start()
    {
        gameObject.SetActive(false); // Ẩn hitbox lúc đầu
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng có Tag "Player" không
        if (other.CompareTag("Player"))
        {
            // Gây sát thương
            Hp health = other.GetComponent<Hp>();
            if (health != null)
            {
                health.TakeDamage(10);
            }

            // Knockback (đẩy lùi Player)
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
                rb.AddForce(knockbackDir * knockbackForce);
            }

            Debug.Log("Hit Player: " + other.name);
        }
    }

    // **Gọi từ Animation Event để kích hoạt Hitbox**
    public void ActivateHitbox()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateHitbox()
    {
        gameObject.SetActive(false);
    }
}

