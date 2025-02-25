using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu va chạm với Player
        {
            Hp player = other.GetComponent<Hp>();
            if (player != null)
            {
                player.TakeDamage(10);
            }
        }
    }
}
