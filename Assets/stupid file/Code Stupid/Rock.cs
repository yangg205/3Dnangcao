using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    public int damage = 50;
    public float explosionForce = 500f;
    public float explosionRadius = 5f;
    public GameObject explosionEffect;
    public LayerMask damageableLayers;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Xóa đá sau một thời gian nếu không va chạm
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        // Tạo hiệu ứng nổ
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Gây sát thương và đẩy lùi các đối tượng xung quanh
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, damageableLayers);
        
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<Hp>()?.TakeDamage(damage);
            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject); // Xóa viên đá sau khi nổ
    }
}