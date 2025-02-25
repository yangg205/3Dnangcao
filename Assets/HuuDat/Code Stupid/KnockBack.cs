using UnityEngine;

public class knockback : MonoBehaviour
{
    public float knockbackForce = 5f; // Lực knockback
    public float knockbackDuration =1f; // Thời gian knockback

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu va chạm với Player
        {
            PlayerKnockBack player = other.GetComponent<PlayerKnockBack>();
            if (player != null)
            {
                player.ApplyKnockback(transform.position, knockbackForce);
            }
        }
    }
}
