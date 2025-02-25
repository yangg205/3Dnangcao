using TMPro;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint; // Vị trí đánh
    public float attackRate = 1f; // Tốc độ đánh
    public float attackRange = 1.5f; // Phạm vi đánh
    private float nextAttackTime = 0f;
    public AudioSource soundAudioSource;
    public AudioClip attackSoundClip;

    // Sát thương
    public int damagePerHit = 25;

    public ParticleSystem hitEffect; // Hiệu ứng đánh trúng

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0)) // Nhấn chuột trái để đánh
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        else
        {
            // Reset animation khi không tấn công
            animator.SetBool("Attack", false);
        }
    }

    private void Attack()
    {
        animator.SetBool("Attack", true);
        soundAudioSource.PlayOneShot(attackSoundClip);

        RaycastHit hit;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, attackRange))
        {
            Debug.Log("Hit: " + hit.transform.name);

            // Gây sát thương cho zombie
            ZombieAI zombieAI = hit.collider.GetComponent<ZombieAI>();
            if (zombieAI != null)
            {
                zombieAI.TakeDamage(damagePerHit);
                ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effect.gameObject, effect.main.duration);
            }

            // Gây sát thương cho zombie theo waypoint
            WaypointZombieAI waypointZombieAI = hit.collider.GetComponent<WaypointZombieAI>();
            if (waypointZombieAI != null)
            {
                waypointZombieAI.TakeDamage(damagePerHit);
                ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effect.gameObject, effect.main.duration);
            }
        }
    }
}
